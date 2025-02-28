import { useEffect } from 'react';
import useAuth from '../../hooks/useAuth.hook';

const LogoutPage = () => {
    const {logout} = useAuth();
    useEffect(()=>{
        logout();
        
        
    })
  return (
    <div>
      
    </div>
  )
}

export default LogoutPage
