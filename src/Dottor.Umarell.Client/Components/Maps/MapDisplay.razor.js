export async function initializeMap(container, items, callback) {

    var map = L.map(container, { attributionControl: false })
        .setView([41.8212342, 12.4594432], 13);

    var bounds = L.latLngBounds()

    L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
        maxZoom: 19,
        //attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
    }).addTo(map);

    var umarellIcon = L.icon({
        iconUrl: "/images/umarell_marker.png",
        iconSize: [50, 100],
        iconAnchor: [25, 99],
        popupAnchor: [0, -95] 
    });

    items.forEach(item => {
        // creazione marker
        //
        var latLng = [item.latitude, item.longitude];
        var marker = L.marker(latLng, { icon: umarellIcon });
        marker.addTo(map);
        bounds.extend(latLng);

        marker.on('click', function (e) {
             callback.invokeMethodAsync("SetMarkerInfo", item.id)
                    .then(markerInfoElement => {
                        // clono l'elemento html per non modificare/alterare quello originale
                        //
                        const clonedInfo = markerInfoElement.cloneNode(true);
                        // lo rendo visibile
                        //
                        clonedInfo.setAttribute("style", "display: block;");
                        marker.bindPopup(clonedInfo).openPopup();


                        //// gestione pulsanti
                        ////
                        //var buttons = clonedInfo.querySelectorAll("button");
                        //if (buttons) {
                        //    buttons.forEach(button => {
                                
                        //        button.addEventListener("click", () => {
                        //            // notifico al codice blazor il pulsante che è stato premuto
                        //            //
                        //            callback.invokeMethodAsync("MarkerInfoButtonClick", id, button.name);
                        //        });
                        //    })
                        //}

                    });

        });
    });

    map.fitBounds(bounds);


}
