import { Routes, Route, Link, useLocation } from 'react-router-dom';
import GetLine from './Line';
import GetJourney from './Journey';
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
                </nav>
            </header>

            <main className="main-content">
                <Routes>
                    <Route path="/journey" element={<GetJourney />} />
                    <Route path="/lines" element={<GetLine />} />
                    <Route path="*" element={<GetJourney />} />
                </Routes>
            </main>
        </div>
    );
}

export default App;
