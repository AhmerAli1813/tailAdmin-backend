import { useState } from "react";
import { PATH_USER } from "../../routes/paths";
import DatePicker from "../general/DatePicker";
import { useNavigate } from "react-router-dom";
import { allowedRolesForUpdateArray } from "../../auth/auth.utils";
import useAuth from "../../hooks/useAuth.hook";
import Button from "../general/Button";
import AuthSpinner from "../general/AuthSpinner";
import {  IAuthUserFilter, IAuthUserList } from "../../types/auth.types";
import { useForm } from "react-hook-form";
import * as Yup from 'yup';
import InputField from "../general/InputField";
import Select2 from "../general/Select2";
import { yupResolver } from '@hookform/resolvers/yup';
import toast from "react-hot-toast";
import axiosInstance from "../../utils/axiosInstance";
import { USERS_LIST_URL } from "../../utils/globalConfig";
interface UserFilterSectionProps {
  setUsers: React.Dispatch<React.SetStateAction<IAuthUserList[]>>;
  ClearFilter: () => void;
}

const UserFilterSection: React.FC<UserFilterSectionProps> = ({ setUsers, ClearFilter }) => {
  const navigate = useNavigate();

  const { user: loggedInUser } = useAuth();

  const [isDisabled, setIsDisabled] = useState(false);
  const [isLoading, setLoading] = useState(false);
  const [isFiltering, setFiltering] = useState(false);
  const roles = allowedRolesForUpdateArray(loggedInUser);

  const RoleOptions = roles.map((role) => ({
    value: role,
    label: role,
  }));


  // Define the Yup schema
  const FilterSchema = Yup.object().shape({
    email: Yup.string()
      .email("Invalid email address")
      .nullable()
      .transform((value) => (value === "" ? null : value)),
    startDate: Yup.date()
      .nullable()
      .typeError("Invalid start date"),
    endDate: Yup.date()
      .nullable()
      .typeError("Invalid end date"),
    userRoles: Yup.array()
      .of(Yup.string().required("Invalid role"))
      .nullable()
    //.default(null), // Ensure it defaults to null
  });


  // React Hook Form configuration
  const { control, handleSubmit, formState: { errors }, reset } = useForm<IAuthUserFilter>({
    resolver: yupResolver(FilterSchema),
    defaultValues: {
      email: "",
      startDate: null,
      endDate: null,
      userRoles: null, // Matches `string[] | null | undefined`
    },
  });

  const onSubmitForm = async (filter: IAuthUserFilter) => {

    try {
      setFiltering(true);
      setLoading(true);
      setIsDisabled(true)
      const response = await axiosInstance.post<IAuthUserList[]>(USERS_LIST_URL, filter);
      const { data } = response;
      setUsers(data);
    } catch (error) {
      const err = error as { data: string; status: number };
      console.log(err);
      const { status } = err;
      if (status === 401) {
        toast.error('Invalid Username or Password');
      } else {
        toast.error('An Error occurred. Please contact admins');
      }
    } finally {
      setLoading(false);
      setIsDisabled(false)

    }
  };
  const handleReset = () => {
    reset({
      email: "",
      startDate: null,
      endDate: null,
      userRoles: null,
    });
    setFiltering(false);
    ClearFilter();
  };

  if (isLoading) {
    return <AuthSpinner />;
  }

  return (
    <div className="page-header row">
      <div className="col-6"></div>
      <div className="page-header-button mt-1 col-6">
        <button
          type="button"
          data-bs-toggle="collapse"
          data-bs-target="#FilterContainer"
          className="btn btn-filter me-1"
        >
          <i className="fa fa-filter me-1" /> Filter
        </button>
        <button
          type="button"
          onClick={() => navigate(PATH_USER.add)}
          className="btn btn-add"
        >
          <i className="fa fa-plus me-1" /> Add
        </button>
      </div>
      <div className={isFiltering?"card filter-card collapse show col-12":"card filter-card collapse col-12" }  id="FilterContainer">
        <div className="card-body">
          <form onSubmit={handleSubmit(onSubmitForm)}>
            <div className="row">
              <div className="col-md-4 col-6">
                <div className="mb-3">
                  <InputField
                    control={control}
                    label="Email"
                    ClassName="form-control"
                    inputName="email"
                    Placeholder="abc@gmail.com"
                    error={errors.email?.message}
                  />
                </div>
              </div>

              <div className="col-md-4 col-6">
                <Select2
                  control={control}
                  inputName="userRoles"
                  options={RoleOptions}
                  isMulti
                  label="Select Roles"
                />
              </div>

              <div className="col-md-4 col-6">

                <DatePicker
                  label="Start Date"
                  inputName="startDate"
                  ClassName="form-control"
                  control={control}
                  error={errors.startDate?.message}

                />

              </div>

              <div className="col-md-4 col-6">

                <DatePicker
                  label="End Date"
                  control={control}
                  inputName="endDate"
                  ClassName="form-control"
                  error={errors.endDate?.message}

                />

              </div>

              <div className="col-12 filter-button">
                <button
                  type="button"
                  className="btn btn-reset me-1"
                  onClick={handleReset}
                >
                  <i className="fa fa-undo" /> Reset
                </button>
                <Button
                  type="submit"
                  ClassName="btn btn-search"
                  label="Search"
                  loading={isLoading}
                  onClick={() => { }}
                  disabled={isDisabled}
                  Children={<i className="fa fa-search me-1" />}
                />
              </div>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default UserFilterSection;




