import { ReactNode, createContext, useReducer, useCallback, useEffect } from 'react';
import {
  IAuthContext,
  IAuthContextAction,
  IAuthContextActionTypes,
  IAuthContextState,
  ILoginResponseDto,
} from '../types/auth.types';
import { getSession, setSession } from './auth.utils';
import axiosInstance from '../utils/axiosInstance';
import toast from 'react-hot-toast';
import { useNavigate } from 'react-router-dom';
import {
  LOGIN_URL,
  ME_URL,
  PATH_AFTER_LOGIN,
  PATH_AFTER_LOGOUT,
} from '../utils/globalConfig';

// We need a reducer function for useReducer hook
const authReducer = (state: IAuthContextState, action: IAuthContextAction) => {
  if (action.type === IAuthContextActionTypes.LOGIN) {
    return {
      ...state,
      isAuthenticated: true,
      isAuthLoading: false,
      user: action.payload,
    };
  }
  if (action.type === IAuthContextActionTypes.LOGOUT) {
    return {
      ...state,
      isAuthenticated: false,
      isAuthLoading: false,
      user: undefined,
    };
  }
  return state;
};

// We need an initial state object for useReducer hook
const initialAuthState: IAuthContextState = {
  isAuthenticated: false,
  isAuthLoading: true,
  user: undefined,
};

// We create our context here and export it
export const AuthContext = createContext<IAuthContext | null>(null);

// We need an interface for our context props
interface IProps {
  children: ReactNode;
}

// We create a component to manage all auth functionalities and export it and use it
const AuthContextProvider = ({ children }: IProps) => {
  const [state, dispatch] = useReducer(authReducer, initialAuthState);
  const navigate = useNavigate();

  // Initialize Method
  const initializeAuthContext = useCallback(async () => {
    try {
      const token = getSession();
      if (token) {
        // validate accessToken by calling backend
        const response = await axiosInstance.post<ILoginResponseDto>(ME_URL, {
          token,
        });
        // In response, we receive jwt token and user data
        const { newToken, userInfo,responseCode } = response.data;
        if(responseCode==200){

          setSession(newToken);
          dispatch({
            type: IAuthContextActionTypes.LOGIN,
            payload: userInfo,
          });
        }else {
          setSession(null);
          dispatch({
            type: IAuthContextActionTypes.LOGOUT,
          });
        }
      } else {
        setSession(null);
        dispatch({
          type: IAuthContextActionTypes.LOGOUT,
        });
      }
    } catch (error) {
      setSession(null);
      dispatch({
        type: IAuthContextActionTypes.LOGOUT,
      });
    }
  }, []);

  // In start of Application, We call initializeAuthContext to be sure about authentication status
  useEffect(() => {
    console.log('AuthContext Initialization start');
    initializeAuthContext()
      .then(() => console.log('initializeAuthContext was successfull'))
      .catch((error) => console.log(error));
  }, []);
  
  
  // Login Method
  const login = useCallback(async (userName: string, password: string) => {
    const response = await axiosInstance.post<ILoginResponseDto>(LOGIN_URL, {
      userName,
      password,
    });
    toast.success('Login Was Successful');
    // In response, we receive jwt token and user data
    const { newToken, userInfo } = response.data;
    setSession(newToken);
    dispatch({
      type: IAuthContextActionTypes.LOGIN,
      payload: userInfo,
    });
    navigate(PATH_AFTER_LOGIN);
  }, []);
  
  // Logout Method
  const logout = useCallback(() => {
    setSession(null);
    dispatch({
      type: IAuthContextActionTypes.LOGOUT,
    });
    navigate(PATH_AFTER_LOGOUT);
  }, []);
  
  // We create an object for values of context provider
  // This will keep our codes more readable
  const valuesObject = {
    isAuthenticated: state.isAuthenticated,
    isAuthLoading: state.isAuthLoading,
    user: state.user,
    login,
    logout,
  };
  
  return <AuthContext.Provider value={valuesObject}>{children}</AuthContext.Provider>;
};

export default AuthContextProvider;
