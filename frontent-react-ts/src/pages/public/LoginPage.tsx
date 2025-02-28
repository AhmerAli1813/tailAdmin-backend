import * as Yup from 'yup';
import { useForm } from 'react-hook-form';
import {  ILoginDto } from '../../types/auth.types';
import InputField from '../../components/general/InputField';
import { yupResolver } from '@hookform/resolvers/yup';
import useAuth from '../../hooks/useAuth.hook';
import Button from '../../components/general/Button';
import { toast } from 'react-hot-toast';
import { useState } from 'react';

const LoginPage = () => {
  const [loading, setLoading] = useState<boolean>(false);
  const { login } = useAuth();

  const loginSchema = Yup.object().shape({
    userName: Yup.string().required('User Name is required'),
    password: Yup.string().required('Password is required').min(8, 'Password must be at least 8 character'),
  });

  const {
    control,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<ILoginDto>({
    resolver: yupResolver(loginSchema),
    defaultValues: {
      userName: '',
      password: '',
    },
  });

  const onSubmitLoginForm = async (data: ILoginDto) => {
    try {
      setLoading(true);
      await login(data.userName, data.password);
      setLoading(false);
      reset();
    } catch (error) {
      setLoading(false);
      
      const err = error as { data: string; status: number };
      const { status } = err;
      if (status === 401) {
        toast.error('Invalid Username or Password');
      }  
       else if(status==423) {
        toast.error('Your Account is temporary  Block. please contact  admin');
      }
       else if(status==424) {
        toast.error('Wrong Password');
      }
       else if(status==429) {
        toast.error('Too Many Request Attempted');
      }
       
         else {
        toast.error('An Error occurred. Please contact admins');
      }
    }
  };
  //        onSubmit={handleSubmit(onSubmitLoginForm)}
  //          <Button variant='primary' type='submit' label='Login' onClick={() => {}} loading={loading} />
  //        <InputField control={control} label='User Name' inputName='userName' error={errors.userName?.message} />

  return (
    <div className='login-img'>

      {/* PAGE */}
      <div className="container-fluid">
        <div className="page">
          <div>
            {/* CONTAINER OPEN */}
            <div className="col col-login mx-auto text-center">
              <a href="index.html" className="text-center" />
            </div>
            <div className="container-login100">
              <div className="wrap-login100 p-0">
                <div className="card-body">
                  <form className="login100-form validate-form" id="formAuthentication" onSubmit={handleSubmit(onSubmitLoginForm)}>
                    <input type="hidden" asp-for="ReturnUrl" />
                    <span className="login100-form-title">
                      <img src="./assets/images/brand/JsilCRMLogo.png" className="header-brand-img" alt="Logo" />
                    </span>
                    {/* Username Input */}
                    <div className="wrap-input100 " >
                      <InputField control={control} label='User Name'  ClassName='input100' inputName='userName' error={errors.userName?.message} />
                      <span className="focus-input100" />
                      <span className="symbol-input100">
                        <span className="zmdi zmdi-account" aria-hidden="true" />
                      </span>
                    </div>
                    {/* Password Input */}
                    <div className="wrap-input100 " >
                      <InputField inputType="password" control={control} label='Password' ClassName='input100' inputName='password' error={errors.password?.message} />

                      <span className="focus-input100" />
                      <span className="symbol-input100">
                        <i className="zmdi zmdi-lock" aria-hidden="true" />
                      </span>
                    </div>
                    {/* Remember Login Checkbox */}
                    <div className="form-check mt-2">
                      <input type="checkbox" className="form-check-input" asp-for="RememberLogin" />
                      <label className="form-check-label" asp-for="RememberLogin">Remember Me</label>
                    </div>
                    {/* Forgot Password Link */}
                    <div className="text-end pt-1">
                      <p className="mb-0"><a href="forgot-password.html" className="text-primary ms-1">Forgot Password?</a></p>
                    </div>
                    {/* Login Button */}
                    <div className="container-login100-form-btn">
                      <Button variant='primary' ClassName='login100-form-btn' type='submit' label='Login' onClick={() => { }} loading={loading} />
                    </div>

                  </form>
                </div>

              </div>
            </div>
            {/* CONTAINER CLOSED */}
          </div>
        </div>
      </div>
      {/* End PAGE */}

    </div>

  );
};

export default LoginPage;
