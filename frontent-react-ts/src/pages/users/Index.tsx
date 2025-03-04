
import UsersTableSection from '../../components/users-management/UsersTableSection';
import UserFilterSection from '../../components/users-management/UserFilterSection';
import { useEffect, useState } from 'react';
import {  IAuthUserFilter, IAuthUserList} from '../../types/auth.types';
import axiosInstance from '../../utils/axiosInstance';
import toast from 'react-hot-toast';
import Spinner from '../../common/Loader/index';
import { USERS_LIST_URL } from '../../utils/globalConfig';
const UserPage = () => {
  const [users, setUsers] = useState<IAuthUserList[]>([]);
  const [loading, setLoading] = useState<boolean>(false);

  const getUsersList = async () => {
    try {
      const filter :IAuthUserFilter ={
         email:"",
         startDate:null,
         endDate:null,
         userRoles:[]
      }
      setLoading(true);
      const response = await axiosInstance.post<IAuthUserList[]>(USERS_LIST_URL,filter);
      const { data } = response;
      setUsers(data);
      setLoading(false);
    } catch (error) {
      toast.error('An Error happened. Please Contact admins');
      setLoading(false);
      console.log(error)
    }
  };

  useEffect(() => {
    getUsersList();
  }, []);

  if (loading) {
    return (
      <div className='w-full'>
        <Spinner />
      </div>
    );
  }
  return (
    <div className='page-container'>
    {/* <UserChartSection usersList={users}  /> */}
      <UserFilterSection setUsers={setUsers} ClearFilter={getUsersList}/>
      <UsersTableSection usersList={users} onRefresh={getUsersList} />
    
    </div>
  );
};

export default UserPage;
