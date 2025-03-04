import { Suspense, useEffect, useState } from 'react';
import { Toaster } from 'react-hot-toast';
import 'react-confirm-alert/src/react-confirm-alert.css'; // Import css
import Modal from "react-modal";
import Loader from './common/Loader';
import GlobalRouter from './routes';

function App() {
  const [loading, setLoading] = useState<boolean>(true);
  Modal.setAppElement("#root");

  useEffect(() => {
    setTimeout(() => setLoading(false), 500);
  }, []);

  return loading ? (
    <Loader />
  ) : (
    <>
      <Toaster
        position="top-right"
        reverseOrder={false}
        containerClassName="overflow-auto"
      />
      <Suspense fallback={<Loader />}>
        <GlobalRouter />
      </Suspense>
    </>
  );
}

export default App;