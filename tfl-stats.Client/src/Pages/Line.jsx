import React, { useEffect, useState } from 'react';
import './Line.css';

const getStatusClass = (description) => {
    const statusMap = {
        good: 'status-good',
        minor: 'status-minordelay',
        severe: 'status-severdelay',
    };

    if (description.includes('Good')) return statusMap.good;
    if (description.includes('Minor')) return statusMap.minor;
    return statusMap.severe;
};

const isDelayed = (statuses) => {
    for (let status of statuses) {
        if (
            status.statusSeverityDescription.includes('Delay') ||
            status.statusSeverityDescription.includes('Closure') ||
            status.statusSeverityDescription.includes('Suspended')
        ) {
            return true;
        }
    }
    return false;
};

function LineStatusRow({ line, toggleStatusDetails, expandedStatusRows }) {
    const statuses = line.lineStatuses || [];
    const delayed = isDelayed(statuses);

    return [
        <tr key="main">
            <td>{line.name}</td>
            <td onClick={() => toggleStatusDetails(line.id)}>
                {statuses.map((status, i) => (
                    <div
                        className={getStatusClass(status.statusSeverityDescription)}
                        key={i}
                    >
                        {status.statusSeverityDescription}
                    </div>
                ))}
            </td>
        </tr>,

        delayed && expandedStatusRows[line.id] && (
            <tr key="details">
                <td colSpan="2">
                    {statuses
                        .filter((status) => {
                            const keywords = ['Delay', 'Closure', 'Suspended'];
                            return keywords.some((keyword) =>
                                status.statusSeverityDescription.includes(keyword)
                            );
                        })
                        .map((status, i) => (
                            <p key={i}>{status.reason}</p>
                        ))}
                </td>
            </tr>
        )
    ];
}

export default function LineStatusTable() {
    const [lines, setLines] = useState([]);
    const [expandedStatusRows, setExpandedStatusRows] = useState({});
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchLineData = async () => {
            try {
                const response = await fetch('api/Line');
                if (response.ok) {
                    const data = await response.json();
                    setLines(data);
                    setExpandedStatusRows({});
                    setLoading(false);
                } else {
                    setError('Failed to fetch line data.');
                    setLoading(false);
                }
            } catch (error) {
                console.error('Error fetching line data:', error);
                setError('An unexpected error occurred while fetching line data.');
                setLoading(false);
            }
        };

        fetchLineData();
        const interval = setInterval(fetchLineData, 30000);
        return () => clearInterval(interval);
    }, []);

    const toggleStatusDetails = (id) => {
        setExpandedStatusRows((prev) => ({
            ...prev,
            [id]: !prev[id]
        }));
    };

    const renderTable = () => {
        if (loading) {
            return <p><em>Loading... Please wait.</em></p>;
        }

        if (error) {
            return <p><em>{error}</em></p>;
        }

        return (
            <table className="table" aria-labelledby="tableLabel">
                <thead>
                    <tr>
                        <th>Line</th>
                        <th>Status</th>
                    </tr>
                </thead>
                <tbody>
                    {lines.map((line) => (
                        <LineStatusRow
                            key={line.id}
                            line={line}
                            toggleStatusDetails={toggleStatusDetails}
                            expandedStatusRows={expandedStatusRows}
                        />
                    ))}
                </tbody>
            </table>
        );
    };

    return (
        <div className="container">
            {renderTable()}
        </div>
    );
}

//export default LineStatusTable;
