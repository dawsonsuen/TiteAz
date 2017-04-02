CREATE TABLE users (
    id UNIQUEIDENTIFIER PRIMARY key,
    email nvarchar(255) not null,
    first_name nvarchar(255) not null,
    last_name nvarchar(255) not null
);

CREATE TABLE debts (
    id UNIQUEIDENTIFIER PRIMARY key,
    bill_id UNIQUEIDENTIFIER PRIMARY key,
    debit_user_id UNIQUEIDENTIFIER not null,
    credit_user_id UNIQUEIDENTIFIER not null,
    amount DECIMAL(12,0) not null,
    debt_status nvarchar(100) not null
);


CREATE TABLE bills (
    id UNIQUEIDENTIFIER PRIMARY key,
    [description] nvarchar(500) not null,
    created_date DATETIME not null
)