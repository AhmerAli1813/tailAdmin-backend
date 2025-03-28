import { useNavigate } from "react-router-dom";
import { PATH_PUBLIC } from "../../routes/paths";

const UnauthorizedPage = () => {
  const navigate = useNavigate();
  return (
  
      <div className="container text-center">
      <div className="error-template">
        <h2 className="text-dark mb-2">401<span className="fs-20">error</span></h2>
        <h5 className="error-details text-dark">
        You don't have access to requested page
        </h5>
        <div className="text-center">
          <a className="btn btn-primary mt-5 mb-5" onClick={()=>navigate(PATH_PUBLIC.home)} > <i className="fa fa-long-arrow-left" /> Back to Home </a>
        </div>
      </div>
    </div>
  );
};

export default UnauthorizedPage;
