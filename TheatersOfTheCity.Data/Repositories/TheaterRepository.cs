﻿using Dapper;
using SqlKata;
using TheatersOfTheCity.Core.Data;
using TheatersOfTheCity.Core.Domain;
using TheatersOfTheCity.Core.Domain.Filters;
using TheatersOfTheCity.Core.Options;
using TheatersOfTheCity.Data.Helpers;

namespace TheatersOfTheCity.Data.Repositories;

public class TheaterRepository : BaseRepository<Theater>, ITheaterRepository
{
    private Query _getAllQuery;

    public TheaterRepository(RepositoryConfiguration sqlConfiguration) : base(sqlConfiguration)
    {
        _getAllQuery = GetAllQuery();
    }

    public override async Task<IEnumerable<Theater>> GetAllAsync()
    {
        var query = _getAllQuery.MySqlQueryToString();

        var result = (await Connection.QueryAsync<Theater, Contact, Theater>(query, (theater, contact) =>
        {
            theater.Director = contact;
            return theater;
        }, splitOn: nameof(Contact.ContactId)));
        return result;
    }

    public override async Task<(IEnumerable<Theater> data, int count)> PaginateAsync(PaginationFilter paginationFilter, SortFilter? sortFilter, DynamicFilters? dynamicFilters)
    {
        var builder = new QueryBuilder<Performance>(paginationFilter, sortFilter, dynamicFilters, _getAllQuery);

        _getAllQuery = builder.Build();
        var data = await GetAllAsync();
        var count = await GetCount();
        _getAllQuery = GetAllQuery();
        return (data, count);
    }

    private Query GetAllQuery()
    {
        return new Query(TableName).Join(nameof(Contact), nameof(Contact.ContactId),
            nameof(Theater.DirectorId));
    }

    public override async Task<Theater> GetByIdAsync(int id)
    {
        var query = new Query(TableName)
            .Join(nameof(Contact), nameof(Contact.ContactId),
            nameof(Theater.DirectorId)).MySqlQueryToString();

        var result = (await Connection.QueryAsync<Theater, Contact, Theater>(query, (theater, contact) =>
        {
            theater.Director = contact;
            return theater;
        }, splitOn: nameof(Contact.ContactId))).First();
        return result;
    }

    public async Task<Theater> GetTheaterByName(string name)
    {
        var query = new Query(TableName)
            .Where(nameof(Theater.Name), "=", name)
            .Join(nameof(Contact), nameof(Contact.ContactId),
                nameof(Theater.DirectorId)).MySqlQueryToString();

        var result = await Connection.QueryAsync<Theater, Contact, Theater>(query, (theater, contact) =>
        {
            theater.Director = contact;
            return theater;
        }, splitOn: nameof(Contact.ContactId));
        return result.FirstOrDefault();
    }
}