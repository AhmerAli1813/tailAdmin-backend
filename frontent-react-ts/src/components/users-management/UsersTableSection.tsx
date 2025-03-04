import { Link } from 'react-router-dom';
import { IAuthDefaultPassword, IAuthStatusDto, IAuthUserList, RolesEnum } from '../../types/auth.types';
import Button from '../general/Button';
import moment from 'moment';
import { isAuthorizedForUpdateRole } from '../../auth/auth.utils';
import useAuth from '../../hooks/useAuth.hook';
import { useState } from 'react';
import Modal from '../general/Modal';
import UpdateRolePage from '../../pages/users/UpdateRolePage';
import DataTable from 'datatables.net-react';
import DT from 'datatables.net-dt';
import axiosInstance from '../../utils/axiosInstance';
import { User_DefaultPassword_URL, User_Lock_URL } from '../../utils/globalConfig';
import toast from 'react-hot-toast';
import { IResponseDto } from '../../types/App.types';
import Spinner from '../../common/Loader/index';
import { confirmAlert } from 'react-confirm-alert';
interface IProps {
  usersList: IAuthUserList[];
  onRefresh: () => void;
}

const UsersTableSection = ({ usersList, onRefresh }: IProps) => {
  DataTable.use(DT);
  const [loading, setLoading] = useState<boolean>(false);

  const { user: loggedInUser } = useAuth();
  const [selectedUserName, setSelectedUserName] = useState<string | null>(null);
  const [isModalOpen, setIsModalOpen] = useState(false);

  const openModal = (userName: string) => {
    setSelectedUserName(userName);
    setIsModalOpen(true);
  };

  const closeModal = () => {
    setIsModalOpen(false);
    setSelectedUserName(null);
    onRefresh();
  };
  const RoleClassNameCreator = (Role: string) => {
    let className = 'badge ';
    if (Role.match(RolesEnum.SuperAdmin)) {
      className += 'bg-red-100 text-red-400';
    } else if (Role.match(RolesEnum.Admin)) {
      className += 'bg-success-100 text-success-400';
    } else if (Role.match(RolesEnum.Investor)) {
      className += 'bg-indigo-100 text-indigo-400';
    } else if (Role.match(RolesEnum.Salesman)) {
      className += 'bg-sky-100 text-sky-400';
    } else if (Role.match(RolesEnum.AppUser)) {
      className += 'bg-gray-400 text-gray-950';
    }
    return className;
  };
  const confirmLockoutChange = (userName: string, lockoutEnabled: boolean) => {
    const action = lockoutEnabled ? "activate" : "deactivate";
    confirmAlert({
      title: 'User Activation',
      message: `Are you sure to do ${action} user`,
      buttons: [
        {
          label: 'Yes',
          color: "green",
          onClick: () => LockOutUser({ userName, lockoutEnabled })
        },
        {
          label: 'No',
          onClick: () => { }
        }
      ]
    });

  };

  const confirmResetPasswordChange = (userName: string) => {
    confirmAlert({
      title: 'User Reset Password',
      message: `Are you sure to do reset a password`,
      buttons: [
        {
          label: 'Yes',
          color: "green",
          onClick: () => { ResetUserPassword({ userName }) }

        },
        {
          label: 'No',
          onClick: () => { }
        }
      ]
    });

  };

  // <span className={RoleClassNameCreator(user.roles)}>{user.roles}</span>
  const LockOutUser = async (req: IAuthStatusDto) => {
    try {

      setLoading(true);
      var response = await axiosInstance.post(User_Lock_URL, req);
      console.log("Status response", response)
      const resp = response.data as IResponseDto;

      if (resp.isSucceed) {
        toast.success("" + resp?.message)
        onRefresh();

      }
    } catch (error) {
      const err = error as { data: number; status: number };
      const { status } = err;
      if (status === 403) {
        toast.error('You are not allowed to change role of this user');
      } else {
        toast.error('An Error occurred. Please contact admins');
      }

    } finally {
      setLoading(false);

    }

  }
  const ResetUserPassword = async (req: IAuthDefaultPassword) => {
    try {

      setLoading(true);
      var response = await axiosInstance.post(User_DefaultPassword_URL, req);
      const resp = response.data as IResponseDto;

      if (resp.isSucceed) {
        toast.success("" + resp?.message)
        //onRefresh();

      }
    } catch (error) {
      const err = error as { data: number; status: number };
      const { status } = err;
      if (status === 403) {
        toast.error('You are not allowed to change role of this user');
      } else {
        toast.error('An Error occurred. Please contact admins');
      }

    } finally {
      setLoading(false);

    }
  }
  if (loading) {
    return (
      <div className='w-full'>
        <Spinner />
      </div>
    );
  }

  return (

    <div className="page-body rounded-sm border border-stroke bg-white px-5 pt-6 pb-2.5 shadow-default dark:border-strokedark dark:bg-boxdark sm:px-7.5 xl:pb-1">
      <div className="max-w-full overflow-x-auto">
        <DataTable className="w-full table-auto">
          <thead>
            <tr className='bg-gray-2 text-left dark:bg-meta-4'>
              <th className="min-w-[110px] py-4 px-2 font-medium text-black dark:text-white xl:pl-11">S No:</th>
              <th className="min-w-[220px] py-4 px-4 font-medium text-black dark:text-white xl:pl-11">Full Name</th>
              <th className="min-w-[220px] py-4 px-4 font-medium text-black dark:text-white xl:pl-11">Username</th>
              <th className="min-w-[220px] py-4 px-4 font-medium text-black dark:text-white xl:pl-11">Email</th>
              <th className="min-w-[220px] py-4 px-4 font-medium text-black dark:text-white xl:pl-11">Phone Number</th>
              <th className="min-w-[220px] py-4 px-4 font-medium text-black dark:text-white xl:pl-11">User Role</th>
              <th className="min-w-[220px] py-4 px-4 font-medium text-black dark:text-white xl:pl-11">Designation</th>
              <th className="min-w-[220px] py-4 px-4 font-medium text-black dark:text-white xl:pl-11">Status</th>
              <th className="min-w-[220px] py-4 px-4 font-medium text-black dark:text-white xl:pl-11">Created At</th>
              <th className="min-w-[220px] py-4 px-4 font-medium text-black dark:text-white xl:pl-11">Actions</th>
            </tr>
          </thead>
          <tbody>
            {usersList.map((user, index) => (

              <tr key={index}>
                <td className='border-b border-[#eee] py-5 px-2 dark:border-strokedark'>{index + 1}</td>
                <td className='border-b border-[#eee] py-5 px-4 dark:border-strokedark'>{user.fullName}</td>
                <td className='border-b border-[#eee] py-5 px-4 dark:border-strokedark'>{user.userName}</td>
                <td className='border-b border-[#eee] py-5 px-4 dark:border-strokedark'>{user.email}</td>
                <td className='border-b border-[#eee] py-5 px-4 dark:border-strokedark'>{user.phoneNumber}</td>
                <td className='border-b border-[#eee] py-5 px-4 dark:border-strokedark'>
                  <span className={RoleClassNameCreator(user.role)}>{user.role}</span>
                </td>
                <td className='border-b border-[#eee] py-5 px-4 dark:border-strokedark'>{user.designation}</td>

                <td>
                  {
                    user.lockoutEnabled ? (
                      <Button onClick={() => confirmLockoutChange(user.userName, false)} ClassName='text-success border w-auto' label='Active' type="button" loading={loading} disabled={!isAuthorizedForUpdateRole(loggedInUser!.roles[0], user.role)} />
                    ) : (
                      <Button onClick={() => confirmLockoutChange(user.userName, true)} ClassName='text-[#F44325] border w-auto' label='InActive' type="button" loading={loading} disabled={!isAuthorizedForUpdateRole(loggedInUser!.roles[0], user.role)} />
                    )
                  }
                </td>
                <td className='border-b border-[#eee] py-5 px-4 dark:border-strokedark'>{moment(user.createdAt).format('YYYY-MM-DD | HH:mm')}</td>
                <td className='border-b border-[#eee] py-5 px-4 pl-9 dark:border-strokedark xl:pl-11'>
                  <div className='flex items-center space-x-3.5'>

                    <Button
                      onClick={() => openModal(user.userName)}
                      type="button"

                      ClassName="hover:text-primary "
                      Children={<span className='fas fa-user'></span>}
                      loading={loading}
                      disabled={!isAuthorizedForUpdateRole(loggedInUser!.roles[0], user.role)}
                    />
                    <Button
                      onClick={() => confirmResetPasswordChange(user.userName)}
                      type="button"

                      ClassName="hover:text-primary"
                      Children={<span className='fas fa-key'></span>}
                      loading={loading}

                      disabled={!isAuthorizedForUpdateRole(loggedInUser!.roles[0], user.role)}
                    />
                    <Link to={`/users/update/${user.userName}`} className="">
                      <span className='fas fa-pencil'></span>

                    </Link>
                  </div>
                </td>
              </tr>
            ))}
          </tbody>
        </DataTable>
        {/* <table id="dataTable" className="table table-striped table-bordered w-100">
      
        <tbody>
        {usersList.map((user, index) => (
        <tr
          key={index}
          className='grid grid-cols-7 px-2 h-12 my-1 border border-gray-200 hover:bg-gray-200 rounded-md'
        >
          <td >{index + 1}</td>
          <td >{user.fullName}</td>
          <td className=' font-semibold'>{user.userName}</td>
          <td className=' font-semibold'>{user.fatherName}</td>
          <td className=' font-semibold'>{user.email}</td>
          <td className=' font-semibold'>{user.phoneNumber}</td>
          <td className='flex justify-center items-center'>
            <span className={RoleClassNameCreator(user.roles)}>{user.roles}</span>
          </td>
          <td >{moment(user.createdAt).format('YYYY-MM-DD | HH:mm')}</td>
          <td >
            <Button
              label='Update Role'
              onClick={() => openModal(user.userName)}
              type='button'
              variant='primary'
              ClassName='btn-sm'
              disabled={!isAuthorizedForUpdateRole(loggedInUser!.roles[0], user.roles[0])}
            />
            <Link to={`/users/update/${user.userName}` } className='ms-1 btn btn-primary btn-sm'    >
                Edit
            </Link> 
                  </td>
        </tr>
      ))}
        </tbody>
      </table> */}
      </div>
      {/* Modal */}
      <Modal isOpen={isModalOpen} onClose={closeModal} title="Update Role">
        {selectedUserName && <UpdateRolePage onDone={closeModal} SelectUserName={selectedUserName} />}
      </Modal>
    </div>


  );
};

export default UsersTableSection;
