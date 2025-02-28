import useAuth from '../../hooks/useAuth.hook';
//import { useNavigate } from 'react-router-dom';
import { useState } from 'react';
import { PATH_DASHBOARD, PATH_USER } from '../../routes/paths';
import { baseURL } from '../../utils/globalConfig';
import MenuItem from '../general/MenuItem';
import { FaCog, FaTachometerAlt, FaUser } from 'react-icons/fa';
import { adminAccessRoles } from '../../auth/auth.utils';

const Sidebar = () => {
  const { user } = useAuth();
 // const navigate = useNavigate();

  // const handleClick = (url: string) => {
  //   window.scrollTo({ top: 0, left: 0, behavior: 'smooth' });
  //   navigate(url);
  // };
  // ______________HOVER JS start
  const [isHovered, setIsHovered] = useState(false);

  const handleMouseEnter = () => {
    if (document.body.classList.contains("sidenav-toggled")) {
      document.body.classList.add("sidenav-toggled-open");
      setIsHovered(true);
      isHovered
    }
  };

  const handleMouseLeave = () => {
    if (document.body.classList.contains("sidenav-toggled")) {
      document.body.classList.remove("sidenav-toggled-open");
      setIsHovered(false);
      isHovered
    }
  };
  return (
<div className="w-100">
  <div className="app-sidebar__overlay" data-bs-toggle="sidebar" />
  <div className="app-sidebar" onMouseEnter={handleMouseEnter}
      onMouseLeave={handleMouseLeave} >
    <div className="side-header">
      <a className="header-brand1" href="#">
        <img src={`${baseURL}/assets/images/brand/JsilCRMLogo.png`} className="header-brand-img desktop-logo" alt="logo" />
        <img src={`${baseURL}/assets/images/brand/JsilCRMLogo.png`} className="header-brand-img light-logo1" alt="logo" />
        <img src={`${baseURL}/assets/images/brand/js-mini.jpg`} className="header-brand-mini-img light-logo1" alt="logo" />
      </a>{/* LOGO */}
    </div>
    <div className="main-sidemenu">
      <div className="slide-left disabled" id="slide-left">
       
      </div>
      <ul className="side-menu">
        <li className='slide'>
          <h3>Menu</h3>
        </li>
        <MenuItem label="Dashboard" to={PATH_DASHBOARD.dashboard} icon={<FaTachometerAlt />} />
     
        
        <MenuItem label="Setting" Access={adminAccessRoles} userRoles={user?.roles} to="#" icon={<FaCog />}   >
        <MenuItem label="Users"  to={PATH_USER.user} icon={<FaUser />} />
        {/* <MenuItem label="Register" to={PATH_USER.add} icon={<FaUser />} /> */}
        </MenuItem>
      
        {/*Setting Area*/}
       
 
      </ul>
      <div className="slide-right" id="slide-right">
       
      </div>
    </div>
  </div>
</div>


  );
};

export default Sidebar;
