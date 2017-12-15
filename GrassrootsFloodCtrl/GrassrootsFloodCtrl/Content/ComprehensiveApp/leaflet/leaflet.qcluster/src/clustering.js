/**
 * @author Rich Gwozdz
 * @created_date 
 */

var QCluster = (function(module){
	
	// Private methods
	var addPinsWithinRange;
	
	addPinsWithinRange = function(points, index, direction, currentCluster, resolution, tolerance){
	
		var clusterwidth,
			finished,
			searchindex,
			pMax,
			xDis,
			yDis;
			
	
		//Cluster width & heigth are in pixels. So any point within 20 pixels at the zoom level will be clustered.
	    clusterwidth = tolerance; // Cluster region width, all points within this area are clustered
		finished = false; // flag
	    searchindex = index + direction;
	    pMax = points.length;
	
		// Loop thru the points array
		while (!finished)
	    {
	    	
	    	// Stop if outside array bounds
	    	if (searchindex >= pMax || searchindex < 0)
	        {
	            finished = true;
	        }
	        else
	        {
	        	// continue if this point has not yet been placed into an existing cluster
	        	if (!points[searchindex].c)
	            {
	                //find distance between two points ( the initial point and one of the other points 'close' to it).
	                xDis = Math.abs(points[searchindex].x - points[index].x) / resolution;
	                yDis = Math.abs(points[searchindex].y - points[index].y) / resolution;
	                
	                if ( xDis < clusterwidth) // the x distance between the two points is within the cluster tolerance
	                {
	                    if (yDis < clusterwidth) // the y distance between the two points is within the cluster tolerance
	                    {
	                        //add to cluster
	                        currentCluster.points.push(points[searchindex]);
	                      	
	                      	//this point represents a cluster...
	                        currentCluster.c = true;
	                        
	                        points[searchindex].c = true;
						}
	
							
					}
					else // we have reached a point whose x distance from the initial point is > the cluster tolerance
	            	{
						/* We have reached a point whose y distance from the initial point is > the cluster tolerance
						/ we assume subsequent points in the list will also be beyond the cluster tolerance since the 
						/ the list is sorted by location grid (GEOREF)
						*/
	                	finished = true;
	            	}
	
	            }
	            //increment the search index
	       	 	searchindex += direction;
	   		}
	        
	    }	
	};

    // 自己添加的，判断某点是否在一个流域内
	var addPinsWithinBasin = function (/*点数组*/points, /*上一层循环的索引*/index, /*向前还是后*/direction, /*当前索引组*/currentCluster, /*已循环到的流域编码*/basincode) {
	    var clusterwidth,
			finished,
			searchindex,
			pMax,

	    //Cluster width & heigth are in pixels. So any point within 20 pixels at the zoom level will be clustered.
	    finished = false; // flag
	    searchindex = index + direction;
	    pMax = points.length;

	    // Loop thru the points array
	    while (!finished) {

	        // Stop if outside array bounds
	        if (searchindex >= pMax || searchindex < 0) {
	            finished = true;
	        }
	        else {
	            // continue if this point has not yet been placed into an existing cluster
	            if (!points[searchindex].c) {

	                if (parseInt(points[searchindex].options.basincode) === basincode) // 如果在流域内
	                {
	                    //add to cluster
	                    currentCluster.points.push(points[searchindex]);

	                    //this point represents a cluster...
	                    currentCluster.c = true;

	                    points[searchindex].c = true;
	                    
	                }
	            }
	            //increment the search index
	            searchindex += direction;
	        }
	    }
	};
	
	// Public Methods
  module.clusterPoints = function (points, mapBounds, resolution, clusterTolerance) {

    function withinBounds(x, y, xmin, xmax, ymin, ymax) {
      if (x > xmax || x < xmin || y > ymax || y < ymin) {
        return false;
      } else {
        return true;
      }
    }

    var ctr = 0;
    var clusters = [];
    var currentCluster = null;
    var pLength = points.length;

    for (var i = pLength - 1; i >= 0; i--) {
      points[i]['c'] = null
    }

    // loop thru the point array
    for (var index = pLength - 1; index >= 0; index--) {

        if (! points[index].c ) {
            //&&
            ////skip already clustered pins
            //withinBounds( points[index].x, points[index].y,
            //              mapBounds.xmin, mapBounds.xmax,
            //              mapBounds.ymin, mapBounds.ymax)

        currentCluster = {'id': ctr, 'points': [], 'xSum': 0, 'ySum': 0, 'cX': null, 'cY': null};
        ctr++;
        currentCluster.points.push(points[index]);

        if (points[0].options) {

            addPinsWithinBasin(points, index, -1, currentCluster, ctr);

            addPinsWithinBasin(points, index, 1, currentCluster, ctr);

            // Add the cluster to the storage array
            if (currentCluster.points.length == 1) {
                if (currentCluster.points[0].options.basincode == ctr)
                    clusters.push(currentCluster);
            } else {
                for (var j = 0; j < currentCluster.points.length; j++) {
                    if (Number(currentCluster.points[j].options.basincode) != ctr) {
                        currentCluster.points.remove(j);
                        break;
                    }
                }
                clusters.push(currentCluster);
            }
        } else {

            //look backwards in the list for any points within the range, return after we hit a point that exceeds range
            addPinsWithinRange(points, index, -1, currentCluster, resolution, clusterTolerance);

            //look forwards in the list for any points within the range, return after we hit a point that exceeds range
            addPinsWithinRange(points, index, 1, currentCluster, resolution, clusterTolerance);

            clusters.push(currentCluster);
        }
      }
    }

    // Loop thru the created clusters and find the center of all cluster points
    for (var j = 0, iMax = clusters.length; j < iMax; j++) {

        var c = clusters[j];
        var cPointLength = 0;
      // Average the x, y coordinates
      for (var k = 0, kMax = c.points.length; k < kMax; k++) {
          c.xSum += Number(c.points[k].lat || c.points[k].x);
          c.ySum += Number(c.points[k].lng || c.points[k].y);
          cPointLength++;
      }
      c.cX = c.xSum / cPointLength;
      c.cY = c.ySum / cPointLength;

      // delete the coordinate sum properties
      delete c.xSum;
      delete c.ySum;
    }

    return clusters;
  };
	
		// Public Methods
	module.moreThanOneCluster = function(points, resolution, clusterTolerance) {
		
		
		var ctr = 0,
			c,
			clusters = [],
			currentCluster;

        var pLength = points.length;

		for(var i = pLength - 1; i >= 0; i--){
			points[i]['c'] = null
		}
	
		// loop thru the point array
		for(var index = pLength - 1; index >= 0; index--){
			
			if (!points[index].c) //skip already clustered pins
	        {
	        	
	        	currentCluster = {'id': ctr, 'points':[], 'xSum': 0, 'ySum':0, 'cX':null, 'cY':null};
	        	ctr++;
	        	currentCluster.points.push(points[index]);
	        	
	        	//look backwards in the list for any points within the range, return after we hit a point that exceeds range
	        	addPinsWithinRange(points, index, -1, currentCluster, resolution, clusterTolerance);
	 
	            //look forwards in the list for any points within the range, return after we hit a point that exceeds range 
	            addPinsWithinRange(points, index, 1, currentCluster, resolution, clusterTolerance);
	 			
	 			// Add the cluster to the storage array
	 			clusters.push(currentCluster);
	        }
	    }
		
		if(clusters.length > 1) {
			return true;
		}
		
		return false;
	};
	return module;
	
}(QCluster || {}));

Array.prototype.remove = function (dx) {
    if (isNaN(dx) || dx > this.length) { return false; }
    for (var i = 0, n = 0; i < this.length; i++) {
        if (this[i] != this[dx]) {
            this[n++] = this[i]
        }
    }
    this.length -= 1
}

