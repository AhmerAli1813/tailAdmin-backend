import { useEffect, useState, memo } from "react";
import axiosInstance from "../../utils/axiosInstance";
import { FundList_URL } from "../../utils/globalConfig";
import {  IFundListDto, IResponseDto } from "../../types/App.types";
import toast from "react-hot-toast";
import Select2 from "./Select2";
import { Control } from "react-hook-form";

export interface IProps {
    control?: Control<any, any>;
    isMulti?: boolean;
    placeholder?: string;
    error?: string;
    label?: string;
    inputName: string;
    ClassName?: string;
    Required?: boolean;
    setLoading?: (loading: boolean) => void;
    SelectedValue?: string;
}

const FundList: React.FC<IProps> = memo(({
    control,
    isMulti = false,
    placeholder = "Select an option",
    error,
    label,
    Required,
    inputName,
    ClassName,
    setLoading,
    SelectedValue,
    ...rest
}) => {
    const [selectedOption, setSelectedOption] = useState<string | undefined>(SelectedValue);
    const [fundOptions, setFundOptions] = useState<{ label: string; value: string }[]>([]);
    const [isFetched, setIsFetched] = useState(false); // Prevent duplicate fetches

    const getList = async () => {
        if (isFetched) return; // Prevent duplicate API calls
        setIsFetched(true);

        try {
            setLoading?.(true);
            const response = await axiosInstance.get<IResponseDto>(FundList_URL);

            if (response.data.isSucceed) {
                const data = response.data.result as IFundListDto[];
                const options = data.map((item) => ({
                    label: item.code || "Unknown fund",
                    value: item.name || "",
                }));

                setFundOptions(options);
                if (SelectedValue && options.some((opt) => opt.value === SelectedValue)) {
                    setSelectedOption(SelectedValue);
                }
            } else {
                toast.error(response.data.message || "Failed to fetch fund list.");
            }
        } catch (error) {
            toast.error("An error occurred. Please contact the admin.");
        } finally {
            setLoading?.(false);
        }
    };

    useEffect(() => {
        console.log("Fetching Fund list...");
        getList();
    }, []);

    return (
        <div className={`Fund-list-wrapper ${ClassName || ""}`}>
            <Select2
                label={label}
                className={ClassName}
                options={fundOptions}
                Required={Required}
                control={control}
                inputName={inputName}
                error={error}
                placeholder={placeholder}
                isMulti={isMulti}
                DefaultValue={selectedOption}
                {...rest}
            />
        </div>
    );
});

export default FundList;
