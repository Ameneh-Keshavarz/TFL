import React from "react";
import data from "../Data/central.json"; 
import { LineSegment, StationMarker, StationName } from "../Components/LineDiagram.jsx";

export default function CentralLineFromJson() {
    const SCALE = 20;

    const stops = data
        .filter(el => el.Type === "marker")
        .map((el, i) => ({
            id: i,
            X: el.Col * SCALE,
            Y: el.Row * SCALE,
        }));

    const connections = data
        .filter(el => el.Type === "trackSection")
        .map((el, i) => ({
            id: i,
            X0: el.Col * SCALE,
            Y0: (el.Row-1) * SCALE,
            X1: el.ColEnd * SCALE,
            Y1: el.Row * SCALE,
        }));

    const names = data
        .filter(el => el.Type === "stationName")
        .map((el, i) => ({
            id: i,
            X: el.Col * SCALE,
            Y: el.Row * SCALE,
            name: el.Name ?? `Station ${i + 1}`, 
            url: el.Url
        }));

    const xs = [
        ...stops.map(s => s.X),
        ...connections.flatMap(c => [c.X0, c.X1]),
        ...names.map(n => n.X + 250),
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

    const padding = 12;
    const vbX = minX - padding;
    const vbY = minY - padding;
    const vbW = (maxX - minX) + padding * 2;
    const vbH = (maxY - minY) + padding * 2;

    return (
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
                border: "2px solid green"
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
                />
            ))}
        </svg>
    );
}
