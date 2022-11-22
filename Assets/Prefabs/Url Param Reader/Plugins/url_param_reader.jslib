mergeInto(LibraryManager.library, {

	GetJsonFromUrl: function() {	 
		var getUrlParameter = function getUrlParameter() 
		{
			var sPageURL = window.location.search.substring(1),
				sURLVariables = sPageURL.split('&'),
				sParameterName,
				i;
			
			var arr = {};
			for (i = 0; i < sURLVariables.length; i++) 
			{
				sParameterName = sURLVariables[i].split('=');
				
				if (sParameterName[1] === undefined)
				{
					continue;
				}
				
				arr[sParameterName[0]] = decodeURIComponent(sParameterName[1]);
			}
			
			return JSON.stringify(arr);
		};
		
		var json = getUrlParameter();

		Module.SendMessage('Url Param Reader', 'RecieveJson', json); 	
	},
 
});