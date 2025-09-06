import { Routes, Route, Link, useLocation } from 'react-router-dom';
import LineStatusTable from './Pages/Line';
import JourneyPlanner from './Pages/Journey';
import LineSelector from './Pages/LineSelector';
import CentralLineFromJson from './Components/LineDiagramFetcher';

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
                        to="/LineSelector"
                        className={`nav-link ${location.pathname === '/LineSelector' ? 'active' : ''}`}
                    >
                        Line Selector
                    </Link>
                </nav>
            </header>

            <main className="main-content">
                <Routes>
                    <Route path="/" element={<JourneyPlanner />} />
                    <Route path="/journey" element={<JourneyPlanner />} />
                    <Route path="/lines" element={<LineStatusTable />} />
                    <Route path="/LineSelector" element={<LineSelector />} />
                    <Route path="/CentralLineDiagramFromJson" element={<CentralLineFromJson />} />

                </Routes>
            </main>
        </div>
    );
}

export default App;
