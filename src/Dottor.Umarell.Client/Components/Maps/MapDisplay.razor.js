export async function initializeMap(container, items, callback) {
    const { Map, InfoWindow  } = await google.maps.importLibrary("maps");
    const { AdvancedMarkerElement, PinElement } = await google.maps.importLibrary("marker");

    var markers = [];

    // inizializzazione mappa
    //
    var map = new Map(container, {
            zoom: 18,
            center: { lat: 45.8851534, lng: 12.3373920 },
            disableDefaultUI: false,
            zoomControl: true,
            mapId: "eb895995cd6a60e5"
        });


    const infoWindow = new InfoWindow();
    var bounds = new google.maps.LatLngBounds();

    items.forEach(item => {
        // creazione marker
        //
        var location = { lat: item.latitude, lng: item.longitude };
        const markerImg = document.createElement("img");
        markerImg.src = "/images/umarell_marker.png";
        const glyphSvgPinElement = new PinElement({
            glyph: markerImg,
        });

        let marker = new AdvancedMarkerElement({
            position: location,
            map: map,
            content: glyphSvgPinElement.element,
        });
        markers.push(marker);
        bounds.extend(location);
        // gestione tooltip con le informazioni del cantiere
        //
        marker.addListener("click", ((id, marker, info) => {
            return () => {
                info.close();
                // chiamo il metodo Blazor che si occupa di valorizzare il tooltip
                // con le info del cantiere selezionato
                //
                callback.invokeMethodAsync("SetMarkerInfo", id)
                    .then(markerInfoElement => {
                        // clono l'elemento html per non modificare/alterare quello originale
                        //
                        const clonedInfo = markerInfoElement.cloneNode(true);
                        // lo rendo visibile
                        //
                        clonedInfo.setAttribute("style", "display: block;");
                        info.setContent(clonedInfo);
                        info.open(map, marker);
                        // gestione chiusura tooltip
                        //
                        google.maps.event.addListener(map, 'click', () => {
                            info.close();
                            clonedInfo.remove();
                        });

                        // gestione pulsanti
                        //
                        var buttons = clonedInfo.querySelectorAll("button");
                        if (buttons) {
                            buttons.forEach(button => {

                                button.addEventListener("click", () => {
                                    // notifico al codice blazor il pulsante che è stato premuto
                                    //
                                    callback.invokeMethodAsync("MarkerInfoButtonClick", id, button.name);
                                });
                            })
                        }

                    });
            }
        })(item.id, marker, infoWindow));

    })

    // Center/Set Zoom of Map to cover all visible Markers
    // https://stackoverflow.com/a/19304625/16405773
    map.fitBounds(bounds);

}
