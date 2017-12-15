window._browserIsNotSupported = true;
if (window.attachEvent) {
	window.attachEvent('onload', function() {
		var lowestSupportedIEVersion = 9;
		//alert(window.LOWEST_IE_VERSION);
		//if(window.LOWEST_IE_VERSION != undefined){
		//  lowestSupportedIEVersion = window.LOWEST_IE_VERSION;
		//}

		var userAgent = navigator.userAgent.toLowerCase(),
			s, o = {};
		var browser = {
			version: (userAgent.match(/(?:firefox|opera|safari|chrome|msie)[\/: ]([\d.]+)/))[1],
			safari: /version.+safari/.test(userAgent),
			chrome: /chrome/.test(userAgent),
			firefox: /firefox/.test(userAgent),
			ie: /msie/.test(userAgent),
			opera: /opera/.test(userAgent)
		} /* 获得浏览器的名称及版本信息 */

		if (browser.ie && browser.version < 8) {
			/* 判断是否为IE 6以上版本，是则执行以下操作 */
			//document.writeln("<p>您使用的是IE "+browser.version+"<\/p>");
			var el = document.createElement('div'),
				elStyle = el.style,
				docBody = document.getElementsByTagName('body')[0],
				linkStyle = 'color:#06F;text-decoration: underline;';
			el.innerHTML = '尊敬的用户：<br />' +
				'您的浏览器版本过低，使用水利科技项目管理系统需要安装更新版本的Internet Explorer浏览器，' +
				'请<a href="http://windows.microsoft.com/zh-cn/internet-explorer/download-ie" style="' + linkStyle + '" target="_blank">下载安装IE' + lowestSupportedIEVersion + '</a>（或更新）。' +
				'也可以在其他浏览器，' +
				'如<a href="browser-install/chrome.exe" style="' + linkStyle + '" target="_blank">谷歌Chrome</a>' +
				'或<a href="browser-install/360se.exe" style="' + linkStyle + '" target="_blank">360安全浏览器</a>' +
				'或<a href="browser-install/sogou_explorer.exe" style="' + linkStyle + '" target="_blank">搜狗浏览器</a>' +
				'或<a href="browser-install/firefox.exe" style="' + linkStyle + '" target="_blank">火狐Firefox</a>中打开水利科技项目管理系统。推荐使用谷歌Chrome浏览器；如有问题请联系：0571-86073375/13666632711/668771（余）;';
			// elStyle.width = '100%';
			elStyle.width = '720px';
			elStyle.color = '#000';
			elStyle.fontSize = '14px';
			elStyle.lineHeight = '180%';
			elStyle.margin = '60px auto';
			elStyle.backgroundColor = '#fffbd5';
			elStyle.border = '1px solid #CCC';
			elStyle.padding = '24px 48px';
			docBody.innerHTML = '';
			docBody.appendChild(el);
		}
	});
}