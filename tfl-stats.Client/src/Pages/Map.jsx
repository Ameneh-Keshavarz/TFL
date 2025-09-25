import { GoogleMap, LoadScript, Marker, Polyline, Circle } from "@react-google-maps/api";
import { useEffect, useState } from "react";
import TrainPrediction from "../Components/TrainPrediction.jsx";
import "./Map.css"

const containerStyle = {
    width: "50%",
    height: "500px",
    marginTop: "24px",
};

const initialCenter = {
    lat: 51.5074,
    lng: -0.1278,
};

export default function Map() {
    const [MapData, setMapData] = useState([]);
    const [arrival, setArrival] = useState([]);
    const [center, setCenter] = useState(initialCenter);

    useEffect(() => {

        if (!navigator.geolocation) return;

        navigator.geolocation.getCurrentPosition(
            ({ coords }) => {
                const loc = { lat: coords.latitude, lng: coords.longitude };
                setCenter(loc);
            },
            () => { } 
        );

        const fetchMapData = async () => {
            try {
                const response = await fetch(`api/MapData`);
                if (response.ok) {
                    const data = await response.json();
                    setMapData(data);
                }
            } catch (error) {
                console.error('Error fetching line diagram data:', error);
            }
        };

        setArrival([]);
        fetchMapData();

    }, []);

    const stations = MapData
        .filter(el => el.Type === "StationData")
        .map((el, i) => ({
            id: i,
            stationId: el.StationId,
            lat: el.Lat,
            lng: el.Lon,
            lines: el.Lines,
        }));

    const connections = MapData
        .filter(el => el.Type === "LineData")
        .map((el, i) => ({
            id: i,
            lineName: el.LineName,
            pathCoordinates: el.LinePoints.map(co => ({
                lat: co.Lat,
                lng: co.Lon
            })),
        }));

    const lineColors = {
        "bakerloo": "#894E24",
        "central": "#DC241F",
        "circle": "#FFD329",
        "district": "#00782A",
        "hammersmith-city": "#D799AF",
        "jubilee": "#6A7278",
        "metropolitan": "#751056",
        "northern": "#000000",
        "piccadilly": "#0019A8",
        "victoria": "#00A0E2",
        "waterloo-city": "#76D0BD",
        "dlr": "#00A4A7",
        "elizabeth": "#9364CC"
    };

    const getLineColor = (lineName) => lineColors[lineName] || "#808080";

    const FetchArrivals = async (stationId, lines) => {

        try {
            const params = new URLSearchParams();
            params.append("stationId", stationId);

            lines.forEach(line => params.append("lines", line));

            const response = await fetch(`api/Arrival?${params.toString()}`);
            if (response.ok) {
                const data = await response.json();
                setArrival(data);
            }
            else {
                setArrival([]);
            }
        }
        catch (error) {
            console.error('Error fetching Arrival data:', error);
            setArrival([]);

        }
    }

    const arrivalsByPlatform = arrival.reduce((acc, pred) => {
        if (!acc[pred.platformName])
            acc[pred.platformName] = [];
        acc[pred.platformName].push(pred);
        return acc;

    }, {});

    const getPlatformNumber = (label) => {
        const matches = label.match(/\d+/g);
        if (!matches || matches.length === 0) return Number.POSITIVE_INFINITY;
        return Number(matches[matches.length - 1]);
    }

    return (
        <LoadScript googleMapsApiKey="AIzaSyAjjrghzDo7QcPf2HbQIeN6w9J-1FfccBg">
            <div className="map-layout">
                <GoogleMap mapContainerStyle={containerStyle} center={center} zoom={12}>
                    {/*    <Marker position={center} />*/}
                    {stations.map(station => (
                        <Circle
                            key={station.id}
                            center={{ lat: station.lat, lng: station.lng }}
                            radius={50}
                            options={{
                                strokeColor: "#FF0000",
                                strokeOpacity: 1.0,
                                strokeWeight: 2,
                                fillColor: "#FF0000",
                                fillOpacity: 1.0
                            }}
                            onClick={() => {
                                FetchArrivals(station.stationId, station.lines);
                            }}
                        />
                    ))}

                    {connections.map(con => (
                        <Polyline
                            key={con.id}
                            path={con.pathCoordinates}
                            options={{
                                strokeColor: getLineColor(con.lineName),
                                strokeOpacity: 0.8,
                                strokeWeight: 4,
                                clickable: true,
                                draggable: false,
                                editable: false,
                                geodesic: true
                            }}
                        />
                    ))}
                </GoogleMap>

                <div className="arrivals">
                    {Object.entries(arrivalsByPlatform)
                        .sort(([a], [b]) => {
                            const na = getPlatformNumber(a);
                            const nb = getPlatformNumber(b);
                            if (na !== nb) return na - nb;
                            return na - nb;
                        }).map(([platform, preds], i) => (
                        <div key={i} className="platform-card">
                            <h3 className="platform-header">{preds[0].stationName} - {platform}</h3>
                            {preds
                                .sort((a, b) => a.timeToStation - b.timeToStation)
                                .map((pred, j) => (
                                    <TrainPrediction
                                        key={j}
                                        lineName={pred.lineName}
                                        platformName={pred.platformName}
                                        destinationName={pred.destinationName}
                                        destinationNaptanId={pred.destinationNaptanId}
                                        timeToStation={pred.timeToStation}
                                    />
                                ))}
                        </div>
                    ))}

                    {arrival.length === 0 && (
                        <p style={{ color: "gray", marginTop: "10px" }}>
                            Click a station to see arrivals
                        </p>
                    )}
                </div>
            </div>
        </LoadScript>
    );
}