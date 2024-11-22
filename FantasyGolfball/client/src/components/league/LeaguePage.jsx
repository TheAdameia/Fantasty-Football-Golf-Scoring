import { useEffect, useState } from "react"
import { useNavigate } from "react-router-dom"
import { GetLeaguesWithVacancies } from "../../managers/leagueManager"
import { LeagueCard } from "./LeagueCard"


export const LeaguePage = () => {
    const [leagues, setLeagues] = useState()
    const navigate = useNavigate()

    const getAndSetLeagues = () => {
        GetLeaguesWithVacancies().then(setLeagues)
    }

    const handleCreate = () => {
        navigate(`/league/create`)
    }

    useEffect(() => {
        getAndSetLeagues()
    }, [])

    return (
        <div>            
            <button
                onClick={() => handleCreate()}
            >Create a league!</button>
            <h2>Leagues looking for players</h2>
            <div>
                {leagues ? leagues.map((league) => {
                    return (
                        <LeagueCard
                            league={league}
                            key={`league-${league.leagueId}`}
                        />
                    )
                }) : <div>No leagues found</div>}
            </div>
        </div>
    )
}