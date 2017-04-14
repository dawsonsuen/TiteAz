CREATE TABLE [dbo].[events]
(
    [id] [bigint] IDENTITY(1,1) NOT NULL,
    [category] [nvarchar](500) NOT NULL,
    [streamid] [uniqueidentifier] NOT NULL,
    [transactionid] [uniqueidentifier] NOT NULL,
    [metadata] [nvarchar](max) NOT NULL,
    [bodytype] [nvarchar](500) NOT NULL,
    [body] [nvarchar](max) NOT NULL,
    [who] [uniqueidentifier] NOT NULL,
    [_when] [datetime] NOT NULL,
    [version] [int] NOT NULL,
    [appversion] [nvarchar](20) NOT NULL
)

GO

create table read_model (
    id bigserial primary key,
    stream_id uuid not null,
    type varchar(500) not null,
    body text not null,
    last_update timestamp not null
);

GO