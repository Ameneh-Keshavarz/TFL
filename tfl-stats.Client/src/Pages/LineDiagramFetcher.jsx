import React from "react";
import { useEffect, useState } from "react";
import PropTypes from "prop-types";

import { LineSegment, StationMarker, StationName } from "../Components/LineDiagramComponents.jsx";
import { TrainPrediction } from "../Components/TrainPrediction.jsx";
import "./LineDiagramFetcher.css";
import { capitalize } from "../../utility/capitalize.jsx";


export default function LineDiagramFetcher({ lineName }) {
    const [lineDiagram, setLineDiagram] = useState([]);
    const [arrival, setArrival] = useState([]);
    const [selectedStationId, setSelectedStationId] = useState(null);
    // const [selectedStationName, setSelectedStationName] = useState("");
    console.log(lineName);

    useEffect(() => {
        const fetchLineData = async () => {
            try {
                const response = await fetch(`api/LineDiagram?lineName=${lineName}`);
                if (response.ok) {
                    const data = await response.json();
                    setLineDiagram(data);
                } 
            } catch (error) {
                console.error('Error fetching line diagram data:', error);
               
            }
        };

        setArrival([]);
        setSelectedStationId(null);
        //setSelectedStationName("");

        fetchLineData();
    }, [lineName]);

    const SCALE = 20;

    const FetchArrivals=async (stationId, line)=>{

        try {
            const response = await fetch(`api/Arrival?lineName=${line}&stationId=${stationId}`);
            if (response.ok) {
                const data = await response.json();
                setArrival(data);
            }
        }
        catch (error) {
            console.error('Error fetching Arrival data:', error);
        }

    }

    const stops = lineDiagram
        .filter(el => el.Type === "marker")
        .map((el, i) => ({
            id: i,
            X: el.Col * SCALE,
            Y: el.Row * SCALE,
        }));

    const connections = lineDiagram
        .filter(el => el.Type === "trackSection")
        .map((el, i) => ({
            id: i,
            X0: el.Col * SCALE,
            Y0: (el.Row-1) * SCALE,
            X1: el.ColEnd * SCALE,
            Y1: el.Row * SCALE,
        }));

    const names = lineDiagram
        .filter(el => el.Type === "stationName")
        .map((el, i) => ({
            id: i,
            X: el.Col * SCALE,
            Y: el.Row * SCALE,
            stationId: el.StationId,
            name: el.Name ?? `Station ${i + 1}`, 
            url: el.Url
        }));

    //const xs = [
    //    ...stops.map(s => s.X),
    //    ...connections.flatMap(c => [c.X0, c.X1]),
    //    ...names.map(n => n.X + 250),
    //];
    const fontSize = 14; 
    const xs = [
        ...stops.map(s => s.X),
        ...connections.flatMap(c => [c.X0, c.X1]),
        ...names.map(n => n.X + n.name.length * fontSize * 0.6),
    ];

    const ys = [
        ...stops.map(s => s.Y),
        ...connections.flatMap(c => [c.Y0, c.Y1]),
        ...names.map(n => n.Y),
    ];

    const minX = xs.length ? Math.min(...xs) : 0;
    const maxX = xs.length ? Math.max(...xs) : 200 * SCALE;
    const minY = ys.length ? Math.min(...ys) : 0;
    const maxY = ys.length ? Math.max(...ys) : 700 * SCALE;

    console.log(minX,maxX,minY,maxY);

    const padding = 12;
    const vbX = minX - padding;
    const vbY = minY - padding;
    const vbW = (maxX - minX) + padding * 2;
    const vbH = (maxY - minY) + padding * 2;

    const arrivalsByPlatform = arrival.reduce((acc, pred) => {
        if (!acc[pred.platformName])
            acc[pred.platformName] = [];
        acc[pred.platformName].push(pred);
        return acc;

    }, {});

    console.log(arrivalsByPlatform);


    return (
        <div className="diagram-layout">
            <svg
                viewBox={`${vbX} ${vbY} ${vbW} ${vbH}`}
                preserveAspectRatio="xMidYMin meet"
                width={420}
                height={900}
                style={{
                    maxWidth: "100%",
                    height: "auto",
                    display: "block",
                    background: "transparent",
                    marginTop: "24px",
                }}
            >
                {connections.map(c => (
                    <LineSegment
                        key={`seg-${c.id}`}
                        x1={c.X0}
                        y1={c.Y0}
                        x2={c.X1}
                        y2={c.Y1}
                        stroke="#E32017"
                        strokeWidth={3}
                    />
                ))}

                {stops.map(s => (
                    <StationMarker
                        key={`stop-${s.id}`}
                        x={s.X}
                        y={s.Y}
                        r={3}
                        stroke="#E32017"
                        strokeWidth={2}
                    />
                ))}

                {names.map(n => (
                    <StationName
                        key={`name-${n.id}`}
                        x={n.X}
                        y={n.Y}
                        name={n.name}
                        url={n.url}
                        onClick={() => {
                            setSelectedStationId(n.stationId);
                            //setSelectedStationName(n.name)
                            FetchArrivals(n.stationId,lineName);
                        }}
                        isSelected={(selectedStationId == n.stationId)}
                    />
                ))}
            </svg>

            {/*{selectedStationName.length !== 0 && (*/}
            {/*    <p style={{ color: "gray", marginTop: "5px" }}>*/}
            {/*        {selectedStationName}*/}
            {/*    </p>*/}
            {/*)}*/}


            <div className="arrivals">
                {Object.entries(arrivalsByPlatform).map(([platform, preds], i) => (
                    <div key={i} className="platform-card">
                        <h3 className="platform-header">{capitalize(lineName)} - {platform}</h3>
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
    );


}



LineDiagramFetcher.propTypes = {
    lineName: PropTypes.string,
};