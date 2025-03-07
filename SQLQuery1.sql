CREATE TABLE CustomerSubmission1 (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    PhoneNo VARCHAR(15) NOT NULL,
    Email VARCHAR(255) NOT NULL,
    Subject VARCHAR(255) NULL,
    Description TEXT NULL
);

ALTER PROCEDURE GetCustomerSubmissions
AS
BEGIN
    SELECT * FROM CustomerSubmission1
    ORDER BY Name;
END;

INSERT INTO CustomerSubmission1 (Name, PhoneNo, Email, Subject, Description)
VALUES 
('Yugaraj', '123456789', 'yugaraj@example.com', 'Inquiry', 'Hi'),
('Ashwin', '123456789', 'ashwin@example.com', 'Request', 'Hi'),
('Sandy', '123456789', 'sandy@example.com', 'Issue', 'Hi'),
('Yuvaraj', '123456789', 'yuvaraj@example.com', 'Support', 'Hi'),
('Ram', '123456789', 'ram@example.com', 'General', 'Hi');

CREATE PROCEDURE sp_CreateCustomerSubmission
    @Name VARCHAR(255),
    @PhoneNo VARCHAR(15),
    @Email VARCHAR(255),
    @Subject VARCHAR(255),
    @Description TEXT
AS
BEGIN
    INSERT INTO CustomerSubmission1 (Name, PhoneNo, Email, Subject, Description)
    VALUES (@Name, @PhoneNo, @Email, @Subject, @Description);
END;

CREATE PROCEDURE sp_UpdateCustomerSubmission
    @Id INT,
    @Name VARCHAR(255),
    @PhoneNo VARCHAR(15),
    @Email VARCHAR(255),
    @Subject VARCHAR(255),
    @Description TEXT
AS
BEGIN
    UPDATE CustomerSubmission1
    SET Name = @Name, PhoneNo = @PhoneNo, Email = @Email, Subject = @Subject, Description = @Description
    WHERE Id = @Id;
END;

CREATE PROCEDURE sp_DeleteCustomerSubmission
    @Id INT
AS
BEGIN
    DELETE FROM CustomerSubmission1 WHERE Id = @Id;
END;