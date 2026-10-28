using PAW.DataAccess.Repositories;
using PAW.Models.DTO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPawTaskRepository, PawTaskRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var tasks = app.MapGroup("/tasks");

tasks.MapGet("/", GetAllTasks);
tasks.MapGet("/{id:int}", GetTask);
tasks.MapPost("/", CreateTask);
tasks.MapPut("/{id:int}", UpdateTask);
tasks.MapDelete("/{id:int}", DeleteTask);

app.Run();

static async Task<IResult> GetAllTasks(IPawTaskRepository repository)
{
    var items = await repository.ReadAsync() ?? [];
    return TypedResults.Ok(items.Select(PawTaskDTO.ConvertFrom).ToList());
}

static async Task<IResult> GetTask(int id, IPawTaskRepository repository)
{
    var entity = await repository.FindAsync(id);
    return entity is null
        ? TypedResults.NotFound()
        : TypedResults.Ok(PawTaskDTO.ConvertFrom(entity));
}

static async Task<IResult> CreateTask(PawTaskDTO dto, IPawTaskRepository repository)
{
    if (string.IsNullOrWhiteSpace(dto.Name))
        return TypedResults.ValidationProblem(new Dictionary<string, string[]> { ["name"] = new[] { "Name is required." } });

    try
    {
        var entity = PawTaskDTO.ConvertTo(dto);
        await repository.CreateAsync(entity);

        var created = PawTaskDTO.ConvertFrom(entity);
        return TypedResults.Created($"/tasks/{created.Id}", created);
    }
    catch (Exception ex)
    {
        return TypedResults.Problem(detail: InnermostMessage(ex), statusCode: StatusCodes.Status409Conflict);
    }
}

static async Task<IResult> UpdateTask(int id, PawTaskDTO dto, IPawTaskRepository repository)
{
    if (string.IsNullOrWhiteSpace(dto.Name))
        return TypedResults.ValidationProblem(new Dictionary<string, string[]> { ["name"] = new[] { "Name is required." } });

    var entity = await repository.FindAsync(id);
    if (entity is null) return TypedResults.NotFound();

    try
    {
        dto.Id = id;
        dto.ApplyTo(entity);
        await repository.UpdateAsync(entity);
        return TypedResults.NoContent();
    }
    catch (Exception ex)
    {
        return TypedResults.Problem(detail: InnermostMessage(ex), statusCode: StatusCodes.Status409Conflict);
    }
}

static async Task<IResult> DeleteTask(int id, IPawTaskRepository repository)
{
    var entity = await repository.FindAsync(id);
    if (entity is null) return TypedResults.NotFound();

    try
    {
        await repository.DeleteAsync(entity);
        return TypedResults.NoContent();
    }
    catch (Exception ex)
    {
        return TypedResults.Problem(detail: InnermostMessage(ex), statusCode: StatusCodes.Status409Conflict);
    }
}

static string InnermostMessage(Exception ex)
{
    while (ex.InnerException != null) ex = ex.InnerException;
    return ex.Message;
}