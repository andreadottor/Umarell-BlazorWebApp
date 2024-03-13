export async function initializeMap(container, callback) {

     var map = L.map(container, { attributionControl: false })
        .setView([45.8851534, 12.3373920], 13);

    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        //attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(map);

    var markersGroup = L.layerGroup();
    map.addLayer(markersGroup);

    // funzione di creazione/posizionamento marker
    //
    var placeMarker = (e) => {
        markersGroup.clearLayers();

        var location = e.latlng

        L.marker(location).addTo(markersGroup);

        // notifico a Blazor le coordinate selezionate
        //
        callback.invokeMethodAsync("SetMarker", location.lat, location.lng);
    }

    map.on('click', placeMarker);

    return markersGroup;
}

// Clear marker from Blazor
//
export function clearMarker(markersGroup) {
    markersGroup.clearLayers();
}

// Change/set marker position from Blazor
//
export function setMarker(markersGroup, position) {
    markersGroup.clearLayers();
    var location = { lat: position.latitude, lng: position.longitude };
    L.marker(location).addTo(markersGroup);
}