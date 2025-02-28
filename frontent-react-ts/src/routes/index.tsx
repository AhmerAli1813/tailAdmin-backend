import { Routes, Route, Navigate } from 'react-router-dom';
import { PATH_DASHBOARD, PATH_PUBLIC, PATH_USER } from './paths';
import AuthGuard from '../auth/AuthGuard';
import { allAccessRoles, managerAccessRoles, adminAccessRoles, superAdminAccessRoles } from '../auth/auth.utils';
import Layout from '../components/layout';
import MyLogsPage from '../pages/logs/MyLogsPage';
import SystemLogsPage from '../pages/logs/SystemLogsPage';
import DashboardPage from '../pages/dashboard'
import LoginPage from '../pages/public/LoginPage';
import NotFoundPage from '../pages/public/NotFoundPage';
import UnauthorizedPage from '../pages/public/UnauthorizedPage';
import UserPage from '../pages/users/Index';
import AddUser from '../pages/users/addUser';
import LogoutPage from '../pages/public/LogoutPage';
import UpdateUser from '../pages/users/updateUser';
import UserProfile from '../pages/users/UserProfile';
import ChangePassword from '../pages/users/changePassword';

const GlobalRouter = () => {
  return (
    <Routes>
      {/* <Route path='' element /> */}
        {/* Public routes */}
        <Route path={PATH_PUBLIC.logout} element={<LogoutPage />} />
        <Route path={PATH_PUBLIC.login} element={<LoginPage />} />
    
      <Route element={<Layout />}>
        

        {/* Protected routes -------------------LoginPage------------------------------- */}
        <Route element={<AuthGuard roles={allAccessRoles} />}>
          <Route index element={<DashboardPage />} />
          <Route path={PATH_DASHBOARD.dashboard} element={<DashboardPage />} />
          <Route path={PATH_DASHBOARD.myLogs} element={<MyLogsPage />} />
          <Route path={PATH_USER.profile} element={<UserProfile/>} />
          <Route path={PATH_USER.changePassword} element={<ChangePassword/>} />
                {/* Catch all (401) */}

        <Route path={PATH_PUBLIC.unauthorized} element={<UnauthorizedPage />} />
        </Route>
        <Route element={<AuthGuard roles={managerAccessRoles} />}>

        </Route>
        <Route element={<AuthGuard roles={adminAccessRoles} />}>
          <Route path={PATH_USER.user} element={<UserPage />} />
          <Route path={PATH_USER.add} element={<AddUser />} />
          <Route path={PATH_USER.update} element={<UpdateUser />} />
          <Route path={PATH_DASHBOARD.systemLogs} element={<SystemLogsPage />} />
        </Route>
        <Route element={<AuthGuard roles={superAdminAccessRoles} />}>

        </Route>
        {/* Protected routes -------------------------------------------------- */}
         

        {/* Catch all (404) */}
        <Route path={PATH_PUBLIC.notFound} element={<NotFoundPage />} />
        <Route path='*' element={<Navigate to={PATH_PUBLIC.notFound} replace />} />
      </Route>
    </Routes>
  );
};

export default GlobalRouter;
