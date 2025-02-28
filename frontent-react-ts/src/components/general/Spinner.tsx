import { baseURL } from "../../utils/globalConfig";

const Spinner = () => {
  return (
    <div id="global-loader">
    <img src= {baseURL+ "/assets/images/loader.svg"} className="loader-img" alt="Loader" />
</div>
  );
};

export default Spinner;
