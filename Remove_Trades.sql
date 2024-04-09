CREATE TRIGGER DeleteOutPutsOnInputsDelete
ON Inputs
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Excluir as linhas correspondentes na tabela OutPuts
    DELETE FROM OutPuts
    WHERE UserId IN (SELECT Id FROM deleted);
END;
