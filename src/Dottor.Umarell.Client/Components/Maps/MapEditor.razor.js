export async function initializeMap(container, callback) {
    const { Map } = await google.maps.importLibrary("maps");
    const { AdvancedMarkerElement } = await google.maps.importLibrary("marker");

    var marker;
    // inizializzazione mappa
    //
    var map = new Map(container, {
        zoom: 17,
        center: { lat: 45.8851534, lng: 12.3373920 },
        disableDefaultUI: true,
        zoomControl: true,
        mapId: "eb895995cd6a60e5"
    });

    // funzione di creazione/posizionamento marker
    //
    var placeMarker = (location) => {
        if (marker) {
            marker.position = location;
        } else {
            marker = new AdvancedMarkerElement({
                position: location,
                map: map
            });
        }
        // TODO: trovare soluzione migliore
        //
        map._marker = marker;
        // notifico a Blazor le coordinate selezionate
        //
        callback.invokeMethodAsync("SetMarker", location.lat(), location.lng());
    }
    // rimango in ascolto del click sulla mappa per recuperare le coordinate
    //
    google.maps.event.addListener(map, 'click', (event) => {
        placeMarker(event.latLng);
    });

    return map;
}

// Clear marker from Blazor
//
export function clearMarker(map) {
    var marker = map._marker;
    if(marker)
        marker.setMap(null);
}

// Change/set marker position from Blazor
//
export function setMarker(map, position) {
    var marker = map._marker;
    var location = { lat: position.latitude, lng: position.longitude };

    if (marker) {
        marker.position = location;
    } else {
        marker = new AdvancedMarkerElement({
            position: location,
            map: map
        });
    }
}