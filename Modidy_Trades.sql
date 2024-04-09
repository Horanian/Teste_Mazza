CREATE TRIGGER UpdateOutPutsOnInputsUpdate
ON Inputs
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Atualizar o campo TradeType na tabela OutPuts
    UPDATE o
    SET o.TradeType = CASE
                            WHEN i.Value > 1000000 AND i.ClientSector = 'Private' THEN 'HIGHRISK'
                            WHEN i.Value > 1000000 AND i.ClientSector = 'Public' THEN 'MEDIUMRISK'
                            WHEN i.Value < 1000000 AND i.ClientSector = 'Public' THEN 'LOWRISK'
                            ELSE 'Uncaterogorized Risks'
                        END
    FROM OutPuts AS o
    INNER JOIN inserted AS i ON o.UserId = i.Id;
END;
