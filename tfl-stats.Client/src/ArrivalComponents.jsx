import React from "react";

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
            onClick={onClick}
        />
    );
}

export function Station({
    x,
    y,
    name,
    markerProps = {},
    labelProps = {},
    onClick
}) {
    const fixedLabelX = 60; 

    return (
        <g onClick={onClick} style={{ cursor: onClick ? "pointer" : "default" }}>
            <StationMarker x={x} y={y} {...markerProps} />
            {name && (
                <text
                    x={fixedLabelX}
                    y={y}
                    fontSize={10}
                    fill="#111"
                    dominantBaseline="middle"
                    textAnchor="start"
                    {...labelProps}
                >
                    {name}
                </text>
            )}
        </g>
    );
}
