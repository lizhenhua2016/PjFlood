/*
 * Leaflet.draw assumes that you have already included the Leaflet library.
 */

L.drawVersion = '0.2.4-dev';

L.drawLocal = {
	draw: {
		toolbar: {
			actions: {
			    title: '取消绘图',
			    text: '取消'
			},
			undo: {
			    title: '删除所绘制图形的最后一个点',
			    text: '删除最后一个点'
			},
			buttons: {
			    polyline: '绘制折线',
			    polygon: '绘制多边形',
			    rectangle: '绘制矩形',
			    circle: '绘制圆形',
			    marker: '标注'
			}
		},
		handlers: {
			circle: {
				tooltip: {
					start: 'Click and drag to draw circle.'
				},
				radius: 'Radius'
			},
			marker: {
				tooltip: {
				    start: '点击地图放置标记'
				}
			},
			polygon: {
				tooltip: {
					start: 'Click to start drawing shape.',
					cont: 'Click to continue drawing shape.',
					end: 'Click first point to close this shape.'
				}
			},
			polyline: {
			    error: '<strong>错误:</strong> 形状边缘不能交叉!',
				tooltip: {
				    start: '单击开始绘制线',
				    cont: '单击继续绘制线',
				    end: '单击最后一点或双击完成绘制线'
				}
			},
			rectangle: {
				tooltip: {
					start: 'Click and drag to draw rectangle.'
				}
			},
			simpleshape: {
				tooltip: {
					end: 'Release mouse to finish drawing.'
				}
			}
		}
	},
	edit: {
		toolbar: {
			actions: {
				save: {
				    title: '保存修改图形',
				    text: '保存'
				},
				cancel: {
				    title: '取消编辑，放弃所有修改图形',
				    text: '取消'
				}
			},
			buttons: {
			    edit: '编辑图形',
			    editDisabled: '无可编辑图形',
			    remove: '删除图形',
			    removeDisabled: '无可删除图形'
			}
		},
		handlers: {
			edit: {
				tooltip: {
				    text: '拖动编辑状态的图形或标记编辑图形',
				    subtext: '单击取消以撤销更改'
				}
			},
			remove: {
				tooltip: {
				    text: '单击一个图形或标记删除'
				}
			}
		}
	}
};
