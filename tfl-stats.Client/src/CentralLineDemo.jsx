import data from "./central-line-diagram.json";
import { Station, LineSegment } from "./ArrivalComponents";

export default function CentralLineDemo() {
    const stops = data.Stops ?? [];
    const conns = data.Connections ?? [];

    const xs = [
        ...stops.map(s => s.X),
        ...conns.flatMap(c => [c.X0, c.X1]),
    ];
    const ys = [
        ...stops.map(s => s.Y),
        ...conns.flatMap(c => [c.Y0, c.Y1]),
    ];

    const minX = xs.length ? Math.min(...xs) : 0;
    const maxX = xs.length ? Math.max(...xs) : 200;
    const minY = ys.length ? Math.min(...ys) : 0;
    const maxY = ys.length ? Math.max(...ys) : 700;

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
            style={{ maxWidth: "100%", height: "auto", display: "block", background: "transparent" }}
        >
           
            {conns.map((c, i) => (
                <LineSegment
                    key={i}
                    x1={c.X0}
                    y1={c.Y0}
                    x2={c.X1}
                    y2={c.Y1}
                    stroke="#E32017"         
                    strokeWidth={3}
                    strokeLinecap="round"
                />
            ))}

           
            {stops.map((s, i) => (
                <Station
                    key={i}
                    x={s.X}
                    y={s.Y}
                    name={data.StationNames?.[i]?.Name}
                    markerProps={{
                        r: 3, 
                        fill: "#fff",
                        stroke: "#111",
                        strokeWidth: 1.2
                    }}
                    labelOffset={{ dx: 10, dy: 0 }}
                    labelProps={{
                        fontSize: 8, 
                        dominantBaseline: "middle",
                        textAnchor: "start" 
                    }}
                />

            ))}
        </svg>
    );
}
