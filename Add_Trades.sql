CREATE TRIGGER UpdateTradeInfo
ON Inputs
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Identificar o último número existente para o campo Trade
    DECLARE @LastTradeNumber INT;
    SELECT @LastTradeNumber = MAX(CAST(SUBSTRING(Trade, 6, LEN(Trade)) AS INT))
    FROM OutPuts;

    -- Se nenhum número for encontrado, atribua 0
    IF @LastTradeNumber IS NULL
        SET @LastTradeNumber = 0;

    -- Atualizar o campo TradeType e Trade na tabela OutPuts
    UPDATE OutPuts
    SET TradeType = CASE
                        WHEN i.Value > 1000000 AND i.ClientSector = 'Private' THEN 'HIGHRISK'
                        WHEN i.Value > 1000000 AND i.ClientSector = 'Public' THEN 'MEDIUMRISK'
                        WHEN i.Value < 1000000 AND i.ClientSector = 'Public' THEN 'LOWRISK'
                        ELSE 'Uncaterogorized Risks'
                    END,
        Trade = 'Trade' + CAST(@LastTradeNumber + ROW_NUMBER() OVER (ORDER BY i.Id) AS VARCHAR)
    FROM OutPuts AS o
    INNER JOIN inserted AS i ON o.UserId = i.Id;
END;
