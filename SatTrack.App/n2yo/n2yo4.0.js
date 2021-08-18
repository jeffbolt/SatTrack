var dayNightOverlay = null;
var sunOverlay = null;
var sArray = new Array();
var footPrint;
var colors = new Array('#FFFF00', '#FF9900', '#FF3366', '#CC00FF', '#990000', '#669900', '#3366FF', '#000066');
var durID; var secID; var drawID; var sunID;
var TOP;
var LEFT;
var DURATION = 300; //seconds 
var REFRESH_DRAW = 3600 //seconds
var REFRESH_DAYNIGHTSUN = 30 //seconds
var selectedSatelliteMarker;
var selectedSatellite = 0;
var intTimezone_now_tzstring;
var map;
var sMarker; //??
var hostname = location.protocol + '//' + location.hostname;
Math.fmod = function (a,b) { return Number((a - (Math.floor(a / b) * b)).toPrecision(8)); };
var firstTime = true;

var selectionCenteredMode;


$(document).ready(function(){
		selectionCenteredMode = readCookie("selectionCenteredMode");
		console.log("read cookie: " + selectionCenteredMode);
		if(selectionCenteredMode=="true")	$("#keepcenter").attr('checked', true);
		else if(selectionCenteredMode=="false")	$("#keepcenter").attr('checked', false);

});

//---------------------------------//
function initialize()
{
	//setConfiguration();
	$("#paneldata").css('visibility', 'hidden');
	var homeLatLng = L.latLng(homeLat,homeLng);

	sArray = new Array();
	var objDate_now = new Date();
	intTimezone_now_tzstring = GetTimezoneString(objDate_now, false);
	firstTime = false;

    if (map != null) {
        map.remove();
        map = null;
    }

	map = L.map('satmap',{
		worldCopyJump:true,
		zoomControl: false,
		scrollWheelZoom: false
	}).setView([homeLat, homeLng], 2);

L.control.zoom({
     position:'topright'
}).addTo(map);

map.on('click', function() {
  if (map.scrollWheelZoom.enabled()) {
    map.scrollWheelZoom.disable();
    }
    else {
    map.scrollWheelZoom.enable();
    }
  });

//	L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
//	attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
//	}).addTo(map);

var mapStyle4 = L.tileLayer('https://server.arcgisonline.com/ArcGIS/rest/services/World_Street_Map/MapServer/tile/{z}/{y}/{x}', {
	attribution: 'Esri'
});

var mapStyle5 = L.tileLayer('https://{s}.tile.opentopomap.org/{z}/{x}/{y}.png', {
	maxZoom: 17,
	attribution: 'Map data: &copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>, <a href="http://viewfinderpanoramas.org">SRTM</a> | Map style: &copy; <a href="https://opentopomap.org">OpenTopoMap</a> (<a href="https://creativecommons.org/licenses/by-sa/3.0/">CC-BY-SA</a>)'
});

mapStyle4.addTo(map);

	var icon = L.icon({
		iconUrl: 'https://www.n2yo.com/img/dot.gif'
	});

	var hMarker = L.marker(homeLatLng, {icon: icon});
	hMarker.addTo(map);
    hMarker.bindPopup("You are here");
    hMarker.on('mouseover', function (e) {
         this.openPopup();
     });
     hMarker.on('mouseout', function (e) {
         this.closePopup();
     });


	var tar = satlist.split("|");
	{
		if (tar.length==1)
		{
			//keepSelectedSatelliteCentered(keepcenterCookie);
			//console.log("zzzzzz " + keepcenterCookie);
			//$("#fit").attr('disabled', true);
			$("#fit").hide();
		}
		else if ((tar.length>1)&&(tar[1]==""))
		{
			//keepSelectedSatelliteCentered(false);
			//$("#fit").attr('disabled', true);
			$("#fit").hide();
		}
	}

	var t = L.terminator();
	t.addTo(map);

	dayNightSun();
	clearInterval (sunID);
	sunID = setInterval ("dayNightSun()", REFRESH_DAYNIGHTSUN*1000);	

	var r = Math.random() * Date.parse(new Date());
//	if(whatsup) var url = "http://"+window.location.hostname+"/sat/jtestw.php?w="+whatsupw+"&hlat="+homeLat+"&hlng="+homeLng+"&r="+r;
	if(whatsup) var url = hostname+"/sat/jtestw.php?w="+whatsupw+"&hlat="+homeLat+"&hlng="+homeLng+"&r="+r;
	//else var url = "http://"+window.location.hostname+"/sat/jtest.php?s="+satlist+"&r="+r;
	else var url = hostname+"/sat/jtest.php?s="+satlist+"&r="+r+"&callback=";
			$("#panelalert").html('Connecting...');
//	document.getElementById("debug").innerHTML=url;	
	$.getJSON(url,
        function(data){
		$("#paneldata").css('visibility', 'hidden');
		$("#satname").css('visibility', 'hidden');
		$("#panelalert").html('Wait...');
		if(whatsup){
			satlist=data[0].strSatList;
			//alert(satlist);
			REFRESH_DRAW = 30;
	//		DURATION = 60;
			DURATION = 30;
			selectedSatellite=0;
		}
		sArray=data;
		drawOrbits();
		clearInterval (drawID);
		//drawID = setInterval ("drawOrbits()", REFRESH_DRAW*1000);	
		drawID = setInterval ("initialize()", REFRESH_DRAW*1000);	
		populateInstantTrk();
		
		clearInterval (durID); 
		durID = setInterval ("populateInstantTrk()", DURATION*1000);
        });
		

}

function drawOrbits()
{
	for (i=0;i<sArray.length;i++)
	{
		var oldDrawingOverlay = sArray[i].orb;
		if (oldDrawingOverlay != "")
		{
			map.removeLayer(oldDrawingOverlay);
		}
			var orb = [];
			for (j=0;j<sArray[i].pos.length;j++)
			{
				var dt = sArray[i].pos[j].d;
				var coord = dt.split("|");
				orb.push(L.latLng(coord[0],coord[1]));

			}

			var c = i%8;


			var drawingOptions = {
		      color: colors[c],
			  weight: 3,
			  opacity: 0.7
		    };


			var drawingOverlay = new L.Wrapped.Polyline(orb, drawingOptions).addTo(map); 

			sArray[i].orb = drawingOverlay;
	}


	if ($('#orbit:checked').val() == 'on')
	{
		showHideOrbits(true);
	}
	else 
	{
		showHideOrbits(false);
	}
}

function showHideOrbits(flag)
{
	if (!flag)
	{
		for (i=0;i<sArray.length;i++)
		{
			var drawingOverlay = sArray[i].orb;
			if (drawingOverlay != null)
			{
				map.removeLayer(drawingOverlay);
			}

		}
	}
	if (flag)
	{
		for (i=0;i<sArray.length;i++)
		{
			var drawingOverlay = sArray[i].orb;
			if (drawingOverlay != null)
			{
				map.addLayer(drawingOverlay);
			}
		}
	}
	$("#orbit").attr('checked', flag);
}

function fitMap(){
	var markers = [];
	for (i=0;i<sArray.length;i++)
	{
		markers.push(sArray[i].mrk);
	}

	var group = new L.featureGroup(markers);

	map.fitBounds(group.getBounds());
}
function keepSelectedSatelliteCentered(flag)
{
	if (flag)
	{
		//map.setZoom(4);
		selectionCenteredMode = "true";
		var expiration_date = new Date();
		var cookie_string = '';
		expiration_date.setFullYear(expiration_date.getFullYear() + 1);
		cookie_string = "selectionCenteredMode=true; expires=" + expiration_date.toUTCString();
		document.cookie = cookie_string;
		console.log("YES! " + cookie_string); 
	}
	else
	{
		selectionCenteredMode = "false";
		var expiration_date = new Date();
		var cookie_string = '';
		expiration_date.setFullYear(expiration_date.getFullYear() + 1);
		cookie_string = "selectionCenteredMode=false; expires=" + expiration_date.toUTCString();
		document.cookie = cookie_string;
		console.log("NO! " + cookie_string); 
	}
	
}

function populateInstantTrk()
{
	var r = Math.random() * Date.parse(new Date());
	//alert(satlist);
	//var url = "http://"+window.location.hostname+"/sat/instant-tracking.php?s="+satlist+"&hlat="+homeLat+"&hlng="+homeLng+"&d="+DURATION+"&r="+r+"&tz=" + intTimezone_now_tzstring+"&O=n2yocom&rnd_str="+rnd_str;
	var url = hostname+"/sat/instant-tracking.php?s="+satlist+"&hlat="+homeLat+"&hlng="+homeLng+"&d="+DURATION+"&r="+r+"&tz=" + intTimezone_now_tzstring+"&O=n2yocom&rnd_str="+rnd_str+"&callback=";
	//alert(url);

	$.getJSON(url,
        function(data){
		//GLog.write("Instant data retrieved!");
		for (i=0;i<sArray.length;i++)
		{
			if(data[i]==null) break;

			for (j=0;j<data[i].pos.length;j++)
			{
				var valArray =  data[i].pos[j].d.split('|');
				var tm = valArray[9];
				sArray[i].ipos[tm] = data[i].pos[j].d;
							//console.log(data[i].pos[j].d);
			}
			// Now add the GMarker to the sArray object
			
			var ctgArr;
			try
			{
				var ctgArr =  sArray[i].pos[0].d.split('|');
			}
			catch(err)
			{
			   //console.log(err.message + ' ' + i + ' ' + sArray[i].id);
			}

			var ctg = ctgArr[5];
			var mrk = createSatelliteMarker(sArray[i].id,ctg);

			if (sArray[i].mrk != "")
			{
				// remove the old marker
				map.removeLayer(sArray[i].mrk);
				if (selectedSatelliteMarker != null)
					map.removeLayer(selectedSatelliteMarker);
			}
			sArray[i].mrk = mrk;
			mrk.addTo(map);
			map.removeLayer(mrk); // hide it, first position would be incorrect

			if (selectedSatellite == 0)
			{
				selectedSatellite = sArray[0].id 
			}

			if(sArray.id == selectedSatellite)
				sArray.sel = 1;
			else 
				sArray.sel = 0;

		}

		selectedSatelliteMarker = createSelectedMarker();
		selectedSatelliteMarker.addTo(map);
		map.removeLayer(selectedSatelliteMarker); // hide it, because it is incorrectly located
		//sArray must be filled by now. Start animation! 
		clearInterval (secID);
		if(sArray[0] != null)
		{
			firstElemIdx = sArray[0].ipos.length - DURATION;
			secID = setInterval ("animateSat()", 1000);
		}
        });

}

function createSatelliteMarker(id,ctg)
{
	if (ctg==undefined) ctg='';
	var centerWorld = L.latLng(0,0);

	var icon = L.icon({
		iconUrl: 'https://www.n2yo.com/inc/saticon.php?t=0&s='+id+'&c='+ctg,
		iconSize:     [30, 30], // size of the icon
	});
	sMarker1 = L.marker(centerWorld, {icon: icon});

	var sn='';
	for (i=0;i<sArray.length;i++)
	{
		if (sArray[i].id==id)
		{
			var sn = '   &nbsp;'+sArray[i].name+' ';
			break;
		}
	}

    sMarker1.bindPopup(sn);
    sMarker1.on('mouseover', function (e) {
         this.openPopup();
     });
     sMarker1.on('mouseout', function (e) {
         this.closePopup();
     });


    sMarker1.on('click', function (e) {
		for (i=0;i<sArray.length;i++)
		{
			var st = sArray[i];
			if (st.id==id)
			{	sArray[i].sel=1;
				selectedSatellite=id;
			}
			else
			{
				sArray[i].sel=0;
			}
		}
     });
	 
	return sMarker1;

}

function createSelectedMarker()
{
	var centerWorld = L.latLng(0,0);

	var icCircle = L.icon({
		iconUrl: 'https://www.n2yo.com/img/sel.gif',
		iconSize:     [16, 16], // size of the icon
		iconAnchor: [10, 20]
	});

	selMarker = L.marker(centerWorld, {icon: icCircle});
	return selMarker;
}
function writeTimeZone()
{
	var dtmp = new Date();
    var tzmin = -dtmp.getTimezoneOffset();
	var tzh = tzmin/60;
	sign = '';
	if(tzh>=0) sign="+";
	$("#ltz").html("GMT"+sign+tzh);
}
function animateSat()
{
	var dt = new Date();
	writeTimeZone()
	//var currTime = Math.ceil(dt.getTime()/1000);
	// Tricky! Originally a match should be found with user's computer clock. But if time was out of sync the main page would show no tracking
	// Now firstElemIdx is just a counter. It is initiated with the first element's time, as per server time.
	firstElemIdx++;
	var currTime = firstElemIdx;
	$("#panelalert").html('');
	$("#paneldata").css('visibility', 'visible'); 
		$("#satname").css('visibility', 'visible');

	for (i=0;i<sArray.length;i++)
	{
		if (sArray[i].ipos[currTime] != null)
		{

			var info = sArray[i].ipos[currTime];
			var coord = info.split("|");
			var mrk = sArray[i].mrk;
			map.addLayer(mrk);
			var vx = round(coord[0],2);
			var vy = round(coord[1],2);
			var vz = round(coord[6],2);
			

			var pos = L.latLng(coord[0],coord[1]);

			mrk.setLatLng(pos);


			sArray[i].mrk = mrk;

			if (sArray[i].id==selectedSatellite)
			{
	
				if(selectionCenteredMode=="true")
				{
					map.panTo(mrk.getLatLng());
				}
				//geocoder.getLocations(pos, showAddress);
				//showLocation("city", "st", "cty");
				selectedSatelliteMarker.setLatLng(pos);
				map.addLayer(selectedSatelliteMarker);
				
				updatePanel(sArray[i].name, sArray[i].id, sArray[i].int_designator, sArray[i].prn, sArray[i].period, info, currTime);
				if(drawFootPrint)
				{
					var tangent = Math.sqrt(vz*(vz+2*6375));

					var centerAngle = Math.asin(tangent/(6375+vz));
					var footPrintRadius=6375*centerAngle; //km
					
					//correction 05/14/2020

					var alpha = Math.acos(6375/(6375 + vz));
					var footPrintRadius2 = alpha * 6375;
					footPrintRadius = footPrintRadius2;


				

					//console.log("footPrintRadius: " + footPrintRadius + " | footPrintRadius1: " + footPrintRadius1);
					var footPrintOld;
					if (footPrint != null)
					{
						footPrintOld = footPrint;
						map.removeLayer(footPrintOld);
					}
/*
						footPrint = new google.maps.Circle({
						center: pos,
						radius: footPrintRadius*1000,
						strokeColor: "#A80000",
						strokeOpacity: 0.7,
						strokeWeight: 1,
						fillColor: "#FFA6A6",
						fillOpacity: 0.5,
						map: map
						});
*/
					  footPrint = L.circle(pos, {
						  color: '#A80000',
						  fillColor: '#FFA6A6',
						  fillOpacity: 0.4,
						  opacity: 0.7,
						  weight: 1,
						  radius: footPrintRadius*1000
					  }).addTo(map); 

					$("#footprint").attr('checked', true);
				}
				else
				{
					if (footPrint != null)
						map.removeLayer(footPrint);
					$("#footprint").attr('checked', false);
				} 
			}

		}
		else
		{
			if (sArray[i].mrk!='')
				map.removeLayer(sArray[i].mrk);

		}

		//GLog.write(currTime + ' ' + info);
		if (mrk != undefined)
		{
			map.addLayer(mrk);
		}

	}

}


function GetTimezoneString(objInputDate, blnJsDateCompat) {
	var objDate = new Date(objInputDate);

	var intDateTZ				= objDate.getTimezoneOffset();
	var strDateTZ_sign			= (intDateTZ > 0 ? "-" : "+")
	var intDateTZ_hours			= Math.floor(Math.abs(intDateTZ) / 60);
	var intDateTZ_minutes		= Math.abs(intDateTZ_hours - (Math.abs(intDateTZ) / 60)) * 60;
	var strDateTZ_normalised	= (blnJsDateCompat ? "UTC" : "GMT") + strDateTZ_sign + PrefixChar(intDateTZ_hours, "0", 2) + (blnJsDateCompat ? "" : ":") + PrefixChar(intDateTZ_minutes, "0", 2);

	return strDateTZ_normalised;
}
function PrefixChar(strValue, strCharPrefix, intLength) {
	var intStrValue_length = String(strValue).length;
	if (intStrValue_length < intLength) {
		for (var intI=0; intI<(intLength-intStrValue_length); ++intI) {
			strValue = strCharPrefix + strValue;
		}
	}
	return strValue;
}

function addZero(x)
{
	if (x<10)
		return '0'+x;
	else
		return x;
}


function round(number,X) { 
	//
X = (!X? 2 : X); 
return Math.round(number*Math.pow(10,X))/Math.pow(10,X); 
}
function dayNightSun()
{
	if (sunOverlay != null)
	{
		map.removeLayer(sunOverlay);
	}
	sunOverlay = getSunOverlay();
	sunOverlay.addTo(map);
}

function getFormatedRA(ra)
{
	ra1 = (ra*24)/360;
	rah=Math.floor(ra1);
	if(rah<10) rah='0'+rah;
	ra2=ra1%1;
	ram=Math.floor(ra2*60);
	if(ram<10) ram='0'+ram;
	ra3=(ra2*60)%1;
	ras=Math.floor(ra3*60);
	if(ras<10) ras='0'+ras;
	var rastr = rah + 'h ' + ram + 'm ' + ras + 's ';
	return rastr;
}

function getFormatedDec(dec)
{
	dech=Math.floor(dec);
	if((dech<10)&&(dech>=0)) dech='0'+Math.abs(dech);
	else if((dech>-10)&&(dech<0)) dech='-0'+Math.abs(dech);
	dec2=dec%1;
	dec2=Math.abs(dec2);
	decm=Math.floor(dec2*60);
	if(decm<10) decm='0'+decm;
	dec3=(dec2*60)%1;
	decs=Math.floor(dec3*60);
	if(decs<10) decs='0'+decs;

	var decstr = dech + "&deg; " + decm + "' " + decs + "'' ";
	return decstr;
}
function setConfiguration()
{
	showHideOrbits(true); // true = show by default
	drawFootPrint = false; // true = draw footprint by default
}

function updatePanel(name, id, int_designator, prn, period, info, currTime)
{
	var d = new Date();
	d.setTime(currTime*1000);
	var coord = info.split("|");
	var city = coord[13]; var country = coord[12]; var region = coord[14];
showLocation(city, region, country);
	var vx = round(coord[0],2);
	vx = vx.toFixed(2);
	var vy = round(coord[1],2);
	vy = vy.toFixed(2);
	var vz = round(coord[6],2);
	vz = vz.toFixed(2);
	// fixed speed
	//console.log(coord[6]);
	var sp = Math.sqrt(398600.8 / (parseFloat(coord[6]) + 6378.135));
	sp = round(sp,2);
	//console.log(sp1);
	//var sp = round(coord[7],2);
	var ra = coord[4];
	var dec = coord[5];
	var az = round(coord[2],1);
	az = az.toFixed(1);
	var el = round(coord[3],1);
	var ut = addZero(d.getUTCHours()) + ':' + addZero(d.getUTCMinutes()) + ':' + addZero(d.getUTCSeconds());
	//var ut = d.getUTCHours() + ':' + d.getUTCMinutes() + ':' + d.getUTCSeconds();
	//var lt = d.toTimeString();;
	var lt = addZero(d.getHours())+':'+addZero(d.getMinutes())+':'+addZero(d.getSeconds());
	$("#noradid").html(id);
	$("#localtime").html(lt);
	$("#utctime").html(ut);
	$("#satname").html('<a href="/satellite/?s='+id+'">'+name+'</a>');
	$("#satlat").html(vx);
	$("#satlng").html(vy);
	$("#sataltkm").html(vz);
	$("#sataltmi").html(round(0.6213712*vz,2));
	$("#satspdkm").html(sp);
	$("#satspdmi").html(round(0.6213712*sp,2));
	$("#satra").html(getFormatedRA(ra));
	$("#satdec").html(getFormatedDec(dec));
	$("#lmst").html(localSiderealTime());
	$("#sataz").text(az);
	$("#period").html(round(period/60,3)+'m');
	if(whatsup) $("#countw").html(sArray.length+' ');	

	if (period<4*3600)
	{
		//$("#prediction").html('<a href=http://www.n2yo.com/passes/?s='+id+'><b>5 DAY PREDICTIONS &#187;</b></a>');
		purl = "/passes/?s="+id;
		var phtml = "<center><button class='sButton' onclick='window.location.href=purl'>10-DAY PREDICTIONS FOR <br/>"+name+"</button></center>";
		$("#prediction").html(phtml);

	}
	else
	{
		$("#prediction").html('&nbsp;');
	}

	if (el>0)
		$("#satel").html('+'+el);
	else
		$("#satel").html(el);
	$("#satazcmp").html(getAzCompass(az));
	if (el>0)
	{
		$("#satel").css({"font-weight" : "normal", "color" : "#000000", "background" : "#FFFF00", "border" : "1px solid #000000"});		
//		$("#sataz").css({"font-weight" : "normal", "color" : "#000000", "background" : "#FFFF00", "border" : "1px solid #000000"});		
//		$("#satazcmp").css({"font-weight" : "normal", "color" : "#000000", "background" : "#FFFF00", "border" : "1px solid #000000"});
	}
	else
	{
		$("#satel").css({"font-weight" : "normal", "color" : "#000000", "background" : "#FFFFFF", "border" : "0px"});		
//		$("#sataz").css({"font-weight" : "normal", "color" : "#000000", "background" : "#FFFFFF", "border" : "0px"});	
//		$("#satazcmp").css({"font-weight" : "normal", "color" : "#000000", "background" : "#FFFFFF", "border" : "0px"});	
	}

	if(coord[10] == 1)
	{
		//$("#satshadow").html("The satellite is in Earth's shadow");
		document.getElementById("satshadow").innerHTML = "The satellite is in Earth's shadow";
		$("#satshadow").css({"font-weight" : "bold", "color" : "#FFFFFF", "background" : "#7E7E7E"});
	}
	else
	{
		$("#satshadow").html("The satellite is in day light");
		$("#satshadow").css({"font-weight" : "normal", "color" : "#000000", "background" : "#FFFF00", "border" : "1px solid #000000"});
	}

}


function showLocation(city, state, country)
{
	if (city != "")
	{
		var text = "";
		if (country=="USA") text = 'Currently over ' + city + ' ' + state + ', ' + country;
		else text = 'Currently over ' + city + ', ' + country;
		$("#satover").html(text);

	}
	else
	{
		$("#satover").html('Retrieving data...');
	}
}

function localSiderealTime()
{
	var jd =  julianDate();
	var lng = homeLng*1.0;
	var D = jd -  2451545.0; 
	var GMST = 280.46061837 + 360.98564736629*D; // degrees
	GMST =  Math.fmod(GMST,360.0);
	var LMST = GMST + lng;
	if (LMST<0) LMST=LMST+360;
	var LMSTStr = getFormatedRA(LMST);
	var GMSTStr = getFormatedRA(GMST);
	return LMSTStr;
}

function julianDate() {
	var dt = new Date();
    MM=dt.getUTCMonth() + 1;
    DD=dt.getUTCDate();
    YY=dt.getUTCFullYear();
    HR=dt.getUTCHours();
    MN= dt.getUTCMinutes();
    SC=dt.getUTCSeconds();
    with (Math) {  
      HR = HR + (MN / 60) + (SC/3600);
      GGG = 1;
      if (YY <= 1585) GGG = 0;
      JD = -1 * floor(7 * (floor((MM + 9) / 12) + YY) / 4);
      S = 1;
      if ((MM - 9)<0) S=-1;
      A = abs(MM - 9);
      J1 = floor(YY + S * floor(A / 7));
      J1 = -1 * floor((floor(J1 / 100) + 1) * 3 / 4);
      JD = JD + floor(275 * MM / 9) + DD + (GGG * J1);
      JD = JD + 1721027 + 2 * GGG + 367 * YY - 0.5;
      JD = JD + (HR / 24);
    }
    return JD;
}
function setCty()
{
	cty["AF"]="Afghanistan [Islamic St.]";
	cty["AL"]="Albania";
	cty["DZ"]="Algeria";
	cty["AS"]="American Samoa";
	cty["AD"]="Andorra";
	cty["AO"]="Angola";
	cty["AI"]="Anguilla";
	cty["AQ"]="Antarctica";
	cty["AG"]="Antigua and Barbuda";
	cty["AR"]="Argentina";
	cty["AM"]="Armenia";
	cty["AW"]="Aruba";
	cty["AC"]="Ascension Island";
	cty["AU"]="Australia";
	cty["AT"]="Austria";
	cty["AZ"]="Azerbaidjan";
	cty["BS"]="Bahamas";
	cty["BH"]="Bahrain";
	cty["BD"]="Bangladesh";
	cty["BB"]="Barbados";
	cty["BY"]="Belarus";
	cty["BE"]="Belgium";
	cty["BZ"]="Belize";
	cty["BJ"]="Benin";
	cty["BM"]="Bermuda";
	cty["BT"]="Bhutan";
	cty["BO"]="Bolivia";
	cty["BA"]="Bosnia-Herzegovina";
	cty["BW"]="Botswana";
	cty["BV"]="Bouvet Island";
	cty["BR"]="Brazil";
	cty["IO"]="British Indian O. Ter.";
	cty["BN"]="Brunei Darussalam";
	cty["BG"]="Bulgaria";
	cty["BF"]="Burkina Faso";
	cty["BI"]="Burundi";
	cty["KH"]="Cambodia";
	cty["CM"]="Cameroon";
	cty["CA"]="Canada";
	cty["CV"]="Cape Verde";
	cty["KY"]="Cayman Islands";
	cty["CF"]="Central African Rep.";
	cty["TD"]="Chad";
	cty["CL"]="Chile";
	cty["CN"]="China";
	cty["CX"]="Christmas Island";
	cty["CC"]="Cocos [Keeling] Isl.";
	cty["CO"]="Colombia";
	cty["KM"]="Comoros";
	cty["CG"]="Congo";
	cty["CK"]="Cook Islands";
	cty["CR"]="Costa Rica";
	cty["HR"]="Croatia";
	cty["CU"]="Cuba";
	cty["CY"]="Cyprus";
	cty["CZ"]="Czech Republic";
	cty["ZR"]="Dem. Rep. of Congo";
	cty["DK"]="Denmark";
	cty["DJ"]="Djibouti";
	cty["DM"]="Dominica";
	cty["DO"]="Dominican Republic";
	cty["TP"]="East Timor";
	cty["EC"]="Ecuador";
	cty["EG"]="Egypt";
	cty["SV"]="El Salvador";
	cty["GQ"]="Equatorial Guinea";
	cty["ER"]="Eritrea";
	cty["EE"]="Estonia";
	cty["ET"]="Ethiopia";
	cty["FK"]="Falkland Isl . [Malvinas]";
	cty["FO"]="Faroe Islands";
	cty["FJ"]="Fiji";
	cty["FI"]="Finland";
	cty["FR"]="France";
	cty["FX"]="France [European Ter.]";
	cty["TF"]="French Southern Terr.";
	cty["GA"]="Gabon";
	cty["GM"]="Gambia";
	cty["GE"]="Georgia";
	cty["DE"]="Germany";
	cty["GH"]="Ghana";
	cty["GI"]="Gibraltar";
	cty["GB"]="Great Britain [UK]";
	cty["GR"]="Greece";
	cty["GL"]="Greenland";
	cty["GD"]="Grenada";
	cty["GP"]="Guadeloupe [Fr.]";
	cty["GU"]="Guam [US]";
	cty["GT"]="Guatemala";
	cty["GG"]="Guernsey [Ch. Isl.]";
	cty["GF"]="Guiana [Fr.]";
	cty["GN"]="Guinea";
	cty["GW"]="Guinea Bissau";
	cty["GY"]="Guyana";
	cty["HT"]="Haiti";
	cty["HM"]="Heard & McDonald Isl.";
	cty["HN"]="Honduras";
	cty["HK"]="Hong Kong";
	cty["HU"]="Hungary";
	cty["IS"]="Iceland";
	cty["IN"]="India";
	cty["ID"]="Indonesia";
	cty["IR"]="Iran";
	cty["IQ"]="Iraq";
	cty["IE"]="Ireland";
	cty["IM"]="Isle of Man";
	cty["IL"]="Israel";
	cty["IT"]="Italy";
	cty["CI"]="Ivory Coast";
	cty["JM"]="Jamaica";
	cty["JP"]="Japan";
	cty["JE"]="Jersey [Ch. Isl.]";
	cty["JO"]="Jordan";
	cty["KZ"]="Kazakstan";
	cty["KE"]="Kenya";
	cty["KI"]="Kiribati";
	cty["KP"]="Korea [north]";
	cty["KR"]="Korea [South]";
	cty["KW"]="Kuwait";
	cty["KG"]="Kyrgyz Republic";
	cty["LA"]="Laos";
	cty["LV"]="Latvia";
	cty["LB"]="Lebanon";
	cty["LS"]="Lesotho";
	cty["LR"]="Liberia";
	cty["LY"]="Libya";
	cty["LI"]="Liechtenstein";
	cty["LT"]="Lithuania";
	cty["LU"]="Luxembourg";
	cty["MO"]="Macau";
	cty["MK"]="Macedonia [former Yug.]";
	cty["MG"]="Madagascar";
	cty["MW"]="Malawi";
	cty["MY"]="Malaysia";
	cty["MV"]="Maldives";
	cty["ML"]="Mali";
	cty["MT"]="Malta";
	cty["MH"]="Marshall Islands";
	cty["MQ"]="Martinique [Fr.]";
	cty["MR"]="Mauritania";
	cty["MU"]="Mauritius";
	cty["YT"]="Mayotte";
	cty["MX"]="Mexico";
	cty["FM"]="Micronesia";
	cty["MD"]="Moldova";
	cty["MC"]="Monaco";
	cty["MN"]="Mongolia";
	cty["MS"]="Montserrat";
	cty["MA"]="Morocco";
	cty["MZ"]="Mozambique";
	cty["MM"]="Myanmar";
	cty["NA"]="Namibia";
	cty["NR"]="Nauru";
	cty["NP"]="Nepal";
	cty["AN"]="Netherland Antilles";
	cty["NL"]="Netherlands";
	cty["NC"]="New Caledonia [Fr.]";
	cty["NZ"]="New Zealand";
	cty["NI"]="Nicaragua";
	cty["NE"]="Niger";
	cty["NG"]="Nigeria";
	cty["NU"]="Niue";
	cty["NF"]="Norfolk Island";
	cty["MP"]="Northern Mariana Isl.";
	cty["NO"]="Norway";
	cty["OM"]="Oman";
	cty["PK"]="Pakistan";
	cty["PW"]="Palau";
	cty["PA"]="Panama";
	cty["PG"]="Papua New Guinea";
	cty["PY"]="Paraguay";
	cty["PE"]="Peru";
	cty["PH"]="Philippines";
	cty["PN"]="Pitcairn";
	cty["PL"]="Poland";
	cty["PF"]="Polynesia [Fr.]";
	cty["PT"]="Portugal";
	cty["PR"]="Puerto Rico";
	cty["QA"]="Qatar";
	cty["CD"]="Rep. Dem. Congo";
	cty["RE"]="Reunion [Fr.]";
	cty["RO"]="Romania";
	cty["RU"]="Russian Federation";
	cty["RW"]="Rwanda";
	cty["LC"]="Saint Lucia";
	cty["SM"]="San Marino";
	cty["SA"]="Saudi Arabia";
	cty["SN"]="Senegal";
	cty["SC"]="Seychelles";
	cty["SL"]="Sierra Leone";
	cty["SG"]="Singapore";
	cty["SK"]="Slovakia [Slovak Rep]";
	cty["SI"]="Slovenia";
	cty["SB"]="Solomon Islands";
	cty["SO"]="Somalia";
	cty["ZA"]="South Africa";
	cty["GS"]="South Georgia and South Sandwich Islands";
	cty["SU"]="Soviet Union";
	cty["ES"]="Spain";
	cty["LK"]="Sri Lanka";
	cty["SH"]="St. Helena";
	cty["KN"]="St. Kitts Nevis Anguilla";
	cty["PM"]="St. Pierre & Miquelon";
	cty["ST"]="St. Tome and Principe";
	cty["VC"]="St. Vincent & Grenadines";
	cty["SD"]="Sudan";
	cty["SR"]="Suriname";
	cty["SJ"]="Svalbard & Jan Mayen Isl.";
	cty["SZ"]="Swaziland";
	cty["SE"]="Sweden";
	cty["CH"]="Switzerland";
	cty["SY"]="Syria";
	cty["TJ"]="Tadjikistan";
	cty["TW"]="Taiwan";
	cty["TZ"]="Tanzania";
	cty["TH"]="Thailand";
	cty["TG"]="Togo";
	cty["TK"]="Tokelau";
	cty["TO"]="Tonga";
	cty["TT"]="Trinidad & Tobago";
	cty["TN"]="Tunisia";
	cty["TR"]="Turkey";
	cty["TM"]="Turkmenistan";
	cty["TC"]="Turks & Caicos Islands";
	cty["TV"]="Tuvalu";
	cty["UG"]="Uganda";
	cty["UA"]="Ukraine";
	cty["AE"]="United Arab Emirates";
	cty["UY"]="Uruguay";
	cty["UM"]="US Minor Outlying Isl.";
	cty["US"]="USA";
	cty["UZ"]="Uzbekistan";
	cty["VU"]="Vanuatu";
	cty["VA"]="Vatican City State";
	cty["VE"]="Venezuela";
	cty["VN"]="Vietnam";
	cty["VG"]="Virgin Islands [Brit]";
	cty["VI"]="Virgin Islands [US]";
	cty["WF"]="Wallis&Futuna Islands";
	cty["EH"]="Western Sahara";
	cty["WS"]="Western Samoa";
	cty["YE"]="Yemen";
	cty["YU"]="Yugoslavia";
	cty["ZM"]="Zambia";
	cty["ZW"]="Zimbabwe";
}
function getAzCompass(deg)
{
	var a=0;
	if((deg>a)&&(deg<a+15))
		return 'N';
	a=a+15;
	if((deg>a)&&(deg<a+15))
		return 'NNE';
	a=a+15;
	if((deg>a)&&(deg<a+15))
		return 'NE';
	a=a+15;
	if((deg>a)&&(deg<a+15))
		return 'NE';
	a=a+15;
	if((deg>a)&&(deg<a+15))
		return 'ENE';
	a=a+15;
	if((deg>a)&&(deg<a+15))
		return 'E';
	a=a+15;
	if((deg>a)&&(deg<a+15))
		return 'E';
	a=a+15;
	if((deg>a)&&(deg<a+15))
		return 'ESE';
	a=a+15;
	if((deg>a)&&(deg<a+15))
		return 'SE';
	a=a+15;
	if((deg>a)&&(deg<a+15))
		return 'SE';
	a=a+15;
	if((deg>a)&&(deg<a+15))
		return 'SSE';
	a=a+15;
	if((deg>a)&&(deg<a+15))
		return 'S';
	a=a+15;
	if((deg>a)&&(deg<a+15))
		return 'S';
	a=a+15;
	if((deg>a)&&(deg<a+15))
		return 'SSW';
	a=a+15;
	if((deg>a)&&(deg<a+15))
		return 'SW';
	a=a+15;
	if((deg>a)&&(deg<a+15))
		return 'SW';
	a=a+15;
	if((deg>a)&&(deg<a+15))
		return 'WSW';
	a=a+15;
	if((deg>a)&&(deg<a+15))
		return 'W';
	a=a+15;
	if((deg>a)&&(deg<a+15))
		return 'W';
	a=a+15;
	if((deg>a)&&(deg<a+15))
		return 'WNW';
	a=a+15;
	if((deg>a)&&(deg<a+15))
		return 'NW';
	a=a+15;
	if((deg>a)&&(deg<a+15))
		return 'NW';
	a=a+15;
	if((deg>a)&&(deg<a+15))
		return 'NNW';
	a=a+15;
	if((deg>a)&&(deg<a+15))
		return 'N';
	a=a+15;
}

function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for(var i=0;i < ca.length;i++) {
        var c = ca[i];
        while (c.charAt(0)==' ') c = c.substring(1,c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length,c.length);
    }
    return null;
}