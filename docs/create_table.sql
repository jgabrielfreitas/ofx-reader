create table [transaction]
(
  id bigint identity,
  bank_id int not null,
  account_id nvarchar(24) not null,
  transaction_id int not null,
  transaction_date datetime2 not null,
  transaction_type nvarchar(24) not null,
  transaction_description nvarchar(1024),
  file_id nvarchar(1024) not null
)
go

create unique index transaction_id_uindex
  on [transaction] (id)
go

alter table [transaction]
  add constraint transaction_pk
    primary key nonclustered (id)
go

