import React from "react";
import "./LineDiagramComponents.css"

export function StationMarker({ x, y, r = 5, fill = "#000", stroke = "#fff", strokeWidth = 2, onClick }) {
    return (
        <circle
            cx={x}
            cy={y}
            r={r}
            fill={fill}
            stroke={stroke}
            strokeWidth={strokeWidth}
            onClick={onClick}
        />
    );
}
StationMarker.displayName = "StationMarker";

export function LineSegment({ x1, y1, x2, y2, stroke = "#333", strokeWidth = 4, strokeLinecap = "round", onClick }) {
    return (
        <line
            x1={x1}
            y1={y1}
            x2={x2}
            y2={y2}
            stroke={stroke}
            strokeWidth={strokeWidth}
            strokeLinecap={strokeLinecap}
            vectorEffect="non-scaling-stroke"
            onClick={onClick}
        />
    );
}
LineSegment.displayName = "LineSegment";

export function StationName({
    x,
    y,
    name,
    dx = 6,          
    dy = 4,          
    labelProps = {},
    onClick,
    isSelected
}) {
    return (
        <text
            x={x + dx}
            y={y + dy}
            fontSize={14}
            textAnchor="start"
            style={{ userSelect: "none", cursor: onClick ? "pointer" : "default" }}
            onClick={onClick}
            {...labelProps}
            className={`station-name ${isSelected ? 'selected' : ''}`}
        >
            {name}
        </text>
    );
}
StationName.displayName = "StationName";
