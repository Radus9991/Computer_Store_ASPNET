import "./Table.component.css";
import { IsLoggedIn } from "../../statuses/UserStatus";
import { useContext, useEffect, useState } from "react";
import { TokenContext } from "../../context/TokenContext";

const Table = ({
  fields,
  data,
  onClickButton = null,
  buttonText = null,
  title = null,
  onClickMoreButton = null,
}) => {
  const { token } = useContext(TokenContext);
  const [page, setPage] = useState(1);
  const [showMoreButton, setShowMoreButton] = useState(true);

  const onMore = () => {
    setPage(page + 1);
  };

  useEffect(() => {
    if (page > 1) {
      (async () => {
        if (onClickMoreButton) {
          const result = await onClickMoreButton(page);
          if (!result) {
            setShowMoreButton(false);
          }
        }
      })();
    }
  }, [page]);

  return (
    <div>
      <h1>{title}</h1>
      <table className="table-data">
        <thead>
          <tr>
            {fields.map((field) => (
              <th key={field.id}>{field.label}</th>
            ))}
            {IsLoggedIn(token) && onClickButton && <th></th>}
          </tr>
        </thead>
        <tbody>
          {data &&
            data.map((element, index) => (
              <tr key={"row_" + index}>
                {fields.map((field) => (
                  <td key={field.id}>{element[field.id]}</td>
                ))}
                {IsLoggedIn(token) && onClickButton && (
                  <td className="custom-button-container">
                    <button
                      className="custom-button"
                      onClick={() => onClickButton(element.id)}
                    >
                      {buttonText}
                    </button>
                  </td>
                )}
              </tr>
            ))}
        </tbody>
        {onClickMoreButton != null && showMoreButton ? (
          <tfoot>
            <td colSpan={fields.length + 1}>
              <button className="custom-button" onClick={onMore}>
                More
              </button>
            </td>
          </tfoot>
        ) : null}
      </table>
    </div>
  );
};

export default Table;
