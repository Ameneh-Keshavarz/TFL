import { Routes, Route, Link, useLocation } from 'react-router-dom';
import LineStatusTable from './Components/Line';
import JourneyPlanner from './Components/Journey';
import CentralLineDiagram from './Pages/CentralLineDiagram';
import CentralLineFromJson from './Pages/CentralLineDiagramFromJson';

import './App.css';

function App() {
    const location = useLocation();

    return (
        <div className="app-container">
            <header className="main-header">
                <h1 className="site-title">🚇 London Travel Assistant</h1>
                <nav className="nav-bar">
                    <Link
                        to="/journey"
                        className={`nav-link ${location.pathname === '/journey' ? 'active' : ''}`}
                    >
                        Journey Planner
                    </Link>
                    <Link
                        to="/lines"
                        className={`nav-link ${location.pathname === '/lines' ? 'active' : ''}`}
                    >
                        Line Status
                    </Link>
                    <Link
                        to="/CentralLineDiagram"
                        className={`nav-link ${location.pathname === '/CentralLineDiagram' ? 'active' : ''}`}
                    >
                        Central Line Diagram
                    </Link>
                    <Link
                        to="/CentralLineDiagramFromJson"
                        className={`nav-link ${location.pathname === '/CentralLineDiagramFromJson' ? 'active' : ''}`}
                    >
                        Central Line Diagram From JSON
                    </Link>
                </nav>
            </header>

            <main className="main-content">
                <Routes>
                    <Route path="/" element={<JourneyPlanner />} />
                    <Route path="/journey" element={<JourneyPlanner />} />
                    <Route path="/lines" element={<LineStatusTable />} />
                    <Route path="/CentralLineDiagram" element={<CentralLineDiagram />} />
                    <Route path="/CentralLineDiagramFromJson" element={<CentralLineFromJson />} />

                </Routes>
            </main>
        </div>
    );
}

export default App;
