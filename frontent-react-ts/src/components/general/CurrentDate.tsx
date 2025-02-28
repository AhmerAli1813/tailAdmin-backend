import React from "react";
import moment from "moment";

interface CurrentDateProps {
  format: string;
}

const CurrentDate: React.FC<CurrentDateProps> = ({ format }) => {
  const currentDate = moment().format(format); // Format current date using moment
  return <span>{currentDate}</span>;
};

export default CurrentDate;
