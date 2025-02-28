import Header from './Header';
import { Outlet, useLocation } from 'react-router-dom';
import Sidebar from './Sidebar';

const Layout = () => {
  const { pathname } = useLocation();

  console.log(pathname);



  return (
    <div id="main-container" className="container-fluid">
      <div className='page'>
        <div className="page-main">
          {/* app-Header */}
          <Header />
          {/* app-Sidebar */}
          <Sidebar /><div className="app-content main-content">
            <div className="side-app">
              {/* CONTAINER */}
              <div className="main-container container-fluid">
                <Outlet  />
              </div>
            </div>
          </div>

        </div>
        {/* FOOTER */}
        <footer className="footer">
          <div className="container">
            <div className="row align-items-center flex-row-reverse">
              <div className="col-md-12 col-sm-12 text-center">
                Copyright © 2025 <a href="#">CRM</a>. Designed  by <a href="#"> JSIL IT Team </a> All rights reserved
              </div>
            </div>
          </div>
        </footer>

      </div>

      {/* Using Outlet, We render all routes that are inside of this Layout */}

    </div>
  );
};

export default Layout;
