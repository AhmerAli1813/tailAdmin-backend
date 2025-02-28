import { Toaster } from 'react-hot-toast';
import GlobalRouter from './routes';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import 'react-confirm-alert/src/react-confirm-alert.css'; // Import css

const App = () => {
  return (
    <div>
      <GlobalRouter />
      <Toaster />
    </div>
  );
};

export default App;
