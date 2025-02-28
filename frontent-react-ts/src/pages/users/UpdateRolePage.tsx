import { useEffect, useState } from 'react';
import Spinner from '../../components/general/Spinner';
import { IAuthUser, IUpdateRoleDto } from '../../types/auth.types';
import axiosInstance from '../../utils/axiosInstance';
import { UPDATE_ROLE_URL, USERS_LIST_URL } from '../../utils/globalConfig';
import { toast } from 'react-hot-toast';
import useAuth from '../../hooks/useAuth.hook';
import { allowedRolesForUpdateArray, isAuthorizedForUpdateRole } from '../../auth/auth.utils';
import Button from '../../components/general/Button';
import Select2 from '../../components/general/Select2';
import * as Yup from 'yup';
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
interface IProps {
  SelectUserName: string
  onDone: () => void; // Callback to notify when done

}
const UpdateRolePage: React.FC<IProps> = ({ SelectUserName, onDone }) => {
  const { user: loggedInUser } = useAuth();
  const [user, setUser] = useState<IAuthUser>();
  const [loading, setLoading] = useState<boolean>(false);
  const [postLoading, setPostLoading] = useState<boolean>(false);
  const roles = allowedRolesForUpdateArray(loggedInUser);

  const RoleOptions = roles.map((role) => ({
    value: role,
    label: role,
  }));
  const getUserByUserName = async () => {
    debugger;
    try {
      setLoading(true);
      const response = await axiosInstance.get<IAuthUser>(`${USERS_LIST_URL}/${SelectUserName}`);
      const { data } = response;
      if (!isAuthorizedForUpdateRole(loggedInUser!.roles[0], data.roles[0])) {
        setLoading(false);
        toast.error('You are not allowed to change role of this user');
      } else {
        setUser(data);
        setLoading(false);
      }
    } catch (error) {

      setLoading(false);
      const err = error as { data: string; status: number };
      const { status } = err;
      if (status === 404) {
        toast.error('UserName not Found!!!!!!!!!!!!!!!!!');
      } else {
        toast.error('An Error occured. Please contact admins');
      }
    }
  };


  useEffect(() => {
    getUserByUserName();
  }, []);

  const UpdateRoleSchema = Yup.object().shape({
    userName: Yup.string().required("User Name is Required"),
    newRole: Yup.string().required("Select a new Role")
  })
  const {
    control,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<IUpdateRoleDto>({
    resolver: yupResolver(UpdateRoleSchema),
    defaultValues: {
      userName: SelectUserName,
      newRole: ""
    },
  });
  const UpdateRole = async (data: IUpdateRoleDto) => {
    debugger;
    try {
      // if (!role || !userName) return;
      // setPostLoading(true);
      // const updateData: IUpdateRoleDto = {
      //   newRole: role,
      //   userName,
      // };

      await axiosInstance.post(UPDATE_ROLE_URL, data);
      toast.success('Role updated Successfully.');
    } catch (error) {
      const err = error as { data: string; status: number };
      const { status } = err;
      if (status === 403) {
        toast.error('You are not allowed to change role of this user');
      } else {
        toast.error('An Error occurred. Please contact admins');
      }
    } finally {
      setPostLoading(false);
      reset()
      onDone();
    }
  };

  if (loading) {
    return (
      <div className='w-full'>
        <Spinner />
      </div>
    );
  }

  return (
    <div className='p-4 w-2/4 mx-auto flex flex-col gap-4'>
      <div className='bg-white p-2 rounded-md flex flex-col gap-4'>

        <div className='border border-dashed border-purple-300 rounded-md'>
          <label className='form-label'>
            <strong>
              UserName:
            </strong>
            <span className='text-2xl font-bold ml-2 px-2 py-1 text-purple-600 rounded-md'>{SelectUserName}</span>
          </label>
          <label className='form-label'>
            <strong>
              Current Role:
            </strong>
            <span className='text-2xl font-bold ml-2 px-2 py-1 text-purple-600 rounded-md'>{user?.roles[0]}</span>
          </label>
        </div>
        <form onSubmit={handleSubmit(UpdateRole)}>
          <Select2
            control={control}
            inputName="newRole"
            options={RoleOptions}
            error={errors.newRole?.message}
            label="Select new Roles"
          />

          <div className='grid grid-cols-2 gap-4 mt-2'>

            <Button label='Update' onClick={() => { }} type="submit" variant='primary' loading={postLoading} />
          </div>
        </form>
      </div>
    </div>
  );
};

export default UpdateRolePage;
