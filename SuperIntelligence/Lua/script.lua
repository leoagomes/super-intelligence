import('SuperIntelligence', 'SuperIntelligence')
import('SuperIntelligence', 'SuperIntelligence.NEAT')
import('SuperIntelligence', 'SuperIntelligence.Data')
import('SuperIntelligence', 'SuperIntelligence.Random')

local MIN_VALUE, MAX_VALUE = 0, 2000000

local Double, Choose, InChance = Random.Double, Random.Choose, Random.InChance

function MakeFirstGeneration(generator, populationSize)
    local generation = Generation(0)
    local genome = Genome(0)

    local inputs = Constants.NetworkInputs
    local outputs = Constants.NetworkOutputs

    -- criacao dos nos de entrada
    local inputIds = {}
    for i=1, inputs do
        local currentId = generator:Innovate()
        table.insert(inputIds, currentId)
        genome:AddNode(Node(currentId, luanet.enum(NodeType, 'Input'), MIN_VALUE))
    end

    -- criacao dos nos de saida
    for i=1, outputs do
        local currentId = generator:Innovate()
        genome:AddNode(Node(currentId, luanet.enum(NodeType, 'Output'), MAX_VALUE))

        for _, id in ipairs(inputIds) do
            genome:AddConnection(Connection(id, currentId, Double() * 4 - 2, true, generator:Innovate()))
        end
    end

    -- criacao de uma especie a partir deste primeiro individuo (utilizando mutacoes)
    local original = Species(genome)
    generation.Species:Add(original)

    for i=1, populationSize do
        local g = genome:Copy()
        g:Mutate(generator)
        g.Id = i

        original:AddGenome(g)
    end

    -- retorna a geracao criada
    return generation
end

function MakeNextGeneration(previousGeneration, innovationGenerator)
    local next = Generation(previousGeneration.Number + 1)
    local genomeId = 0

    -- para cada especie
    for i=0, previousGeneration.Species.Count-1 do
        local oldSpecies = previousGeneration.Species[i]
        local newSpecies = Species(oldSpecies:RandomMember())
        print("got here")
        next.Species:Add(newSpecies)

        -- ordena membros da especie
        local sortedMembers = oldSpecies:SortedMembers()
        local top = sortedMembers[0] -- pega o membro de maior fitness

        -- se a especie tiver mais de 5 individuos, adicionar o top na especie
        if oldSpecies.Members.Count > 5 then
            next:AddGenome(top)
        end

        -- para cada individuo, cruzar com o top
        for i=0, oldSpecies.Members.Count - 1 do
            local genome = oldSpecies.Members[i]

            if top == genome or genome.Fitness <= -4000 then
                -- nao fazer nada
            else
                local child = Genome.CrossOver(top, genome)
                child:Mutate(innovationGenerator)
                child.Id = genomeId
                genomeId = genomeId + 1
                next:AddGenome(child)
            end
        end

        -- chances de cruzamento entre especies
        for i=0, next.Species.Count - 1 do
            if InChance(Generation.InterspeciesMatingChance) then
                local other = next:RandomSpecies()
                local cross = Genome.CrossOver(next.Species[i]:SortedMembers()[0], other:SortedMembers()[0])
                cross.Id = genomeId
                genomeId = genomeId + 1
                next:AddGenome(cross)
            end
        end

        next:RemoveEmptySpecies()

        return next
    end
end
