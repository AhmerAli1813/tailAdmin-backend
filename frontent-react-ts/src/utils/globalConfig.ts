import { PATH_DASHBOARD, PATH_PUBLIC } from '../routes/paths';

// URLS
export const HOST_API_KEY = 'https://localhost:7134/api';
// export const HOST_API_KEY = 'http://localhost:5000/api';
export const baseURL = window.location.origin;
export const REGISTER_URL = '/Account/register';
export const User_Update_URL = '/Account/update';
export const User_Lock_URL = '/Account/lock-user';
export const User_DefaultPassword_URL = '/Account/user-set-default-password';
export const LOGIN_URL = '/Account/login';
export const ME_URL = '/Account/me';
export const USERS_LIST_URL = '/Account/users';
export const USERS_Filter_LIST_URL = '/Account/FilterUsers';
export const UPDATE_ROLE_URL = '/Account/update-role';
export const USERNAMES_LIST_URL = '/Account/usernames';
export const Regional_RelationShip_user_list__URL = '/Account/get-Regional-RelationShip-user-list';
export const LOGS_URL = '/Logs';
export const MY_LOGS_URL = '/Logs/mine';


// Account Routes
export const PATH_AFTER_REGISTER = PATH_PUBLIC.login;
export const PATH_AFTER_LOGIN = PATH_DASHBOARD.dashboard;
export const PATH_AFTER_LOGOUT = PATH_PUBLIC.login;
