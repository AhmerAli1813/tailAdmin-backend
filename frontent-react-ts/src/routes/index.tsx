import { lazy } from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import { PATH_DASHBOARD, PATH_PUBLIC, PATH_USER, PATH_UI } from './paths';
import { allAccessRoles,  adminAccessRoles, superAdminAccessRoles } from '../auth/auth.utils';
import AuthGuard from '../auth/AuthGuard';

const Calendar = lazy(() => import('../pages/Calendar'));
const Chart = lazy(() => import('../pages/Chart'));
const FormElements = lazy(() => import('../pages/Form/FormElements'));
const FormLayout = lazy(() => import('../pages/Form/FormLayout'));
const Profile = lazy(() => import('../pages/Profile'));
const Settings = lazy(() => import('../pages/Settings'));
const Tables = lazy(() => import('../pages/Tables'));
const Alerts = lazy(() => import('../pages/UiElements/Alerts'));
const Buttons = lazy(() => import('../pages/UiElements/Buttons'));
const Layout = lazy(() => import('../layout/DefaultLayout'));


const MyLogsPage = lazy(() => import('../pages/logs/MyLogsPage'));
const SystemLogsPage = lazy(() => import('../pages/logs/SystemLogsPage'));
const DashboardPage = lazy(() => import('../pages/Dashboard/ECommerce'));
const LoginPage = lazy(() => import('../pages/Authentication/SignIn'));
const NotFoundPage = lazy(() => import('../pages/public/NotFoundPage'));
const UnauthorizedPage = lazy(() => import('../pages/public/UnauthorizedPage'));
const UserPage = lazy(() => import('../pages/users/Index'));
const AddUser = lazy(() => import('../pages/users/addUser'));
const UpdateUser = lazy(() => import('../pages/users/updateUser'));
const UserProfile = lazy(() => import('../pages/users/UserProfile'));
const ChangePassword = lazy(() => import('../pages/users/changePassword'));

const GlobalRouter = () => {
  return (
    <Routes>
      {/* Public routes */}
      <Route path={PATH_PUBLIC.login} element={<LoginPage />} />
    
      <Route element={<Layout />}>
        {/* Protected routes */}
        <Route element={<AuthGuard roles={allAccessRoles} />}>
          <Route index element={<DashboardPage />} />
          <Route path={PATH_DASHBOARD.dashboard} element={<DashboardPage />} />
          <Route path={PATH_DASHBOARD.myLogs} element={<MyLogsPage />} />
          <Route path={PATH_USER.profile} element={<UserProfile />} />
          <Route path={PATH_USER.changePassword} element={<ChangePassword />} />
          <Route path={PATH_UI.settings} element={<Settings />} />
          <Route path={PATH_UI.profile} element={<Profile />} />
          <Route path={PATH_UI.formLayout} element={<FormLayout />} />
          <Route path={PATH_UI.formElements} element={<FormElements />} />
          <Route path={PATH_UI.alerts} element={<Alerts />} />
          <Route path={PATH_UI.calendar} element={<Calendar />} />
          <Route path={PATH_UI.chart} element={<Chart />} />
          <Route path={PATH_UI.tables} element={<Tables />} />
          <Route path={PATH_UI.buttons} element={<Buttons />} />
          {/* Catch all (401) */}
          <Route path={PATH_PUBLIC.unauthorized} element={<UnauthorizedPage />} />
        </Route>
        
        <Route element={<AuthGuard roles={adminAccessRoles} />}>
          <Route path={PATH_USER.user} element={<UserPage />} />
          <Route path={PATH_USER.add} element={<AddUser />} />
          <Route path={PATH_USER.update} element={<UpdateUser />} />
          <Route path={PATH_DASHBOARD.systemLogs} element={<SystemLogsPage />} />
        </Route>
        <Route element={<AuthGuard roles={superAdminAccessRoles} />}>

        </Route>
        {/* Protected routes */}
         
        {/* Catch all (404) */}
        <Route path={PATH_PUBLIC.notFound} element={<NotFoundPage />} />
        <Route path='*' element={<Navigate to={PATH_PUBLIC.notFound} replace />} />
      </Route>
    </Routes>
  );
};

export default GlobalRouter;
