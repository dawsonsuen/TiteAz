create table events
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
);


create table read_model (
    [id] [bigint] IDENTITY(1,1) NOT NULL,
    [stream_id] [uniqueidentifier] NOT NULL,
    type nvarchar(500) not null,
    body text not null,
    last_update datetime not null
);
