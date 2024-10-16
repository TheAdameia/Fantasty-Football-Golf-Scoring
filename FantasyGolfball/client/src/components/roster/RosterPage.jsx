import { useEffect, useState } from "react"
import { RosterCard } from "./RosterCard"
import { GetByUserAndLeague } from "../../managers/rosterManager"
import { useAppContext } from "../../contexts/AppContext"
import { GetByWeekAndPlayers } from "../../managers/scoringManager"
import { Button, ButtonDropdown } from "reactstrap"


export const RosterPage = () => {
    const [roster, setRoster] = useState({})
    const [scores, setScores] = useState([])
    const { loggedInUser, globalWeek } = useAppContext()

    const getAndSetRoster = () => {
        let leagueId = 1 // I will need to actually get this value instead of assuming it
        let userId = loggedInUser.id
        GetByUserAndLeague(leagueId, userId).then(setRoster)
    }

    const getAndSetScores = () => {
        const rosterPlayers = roster?.rosterPlayers || []

        const playerIds = rosterPlayers.map(p => p.playerId)
        const playerIdsString = playerIds.join(',')
       
        if (playerIds.length > 0) {
            GetByWeekAndPlayers(globalWeek, playerIdsString).then(setScores)
        }
    }

    useEffect(() => {
        getAndSetRoster()
    }, [])

    useEffect(() => {
        getAndSetScores()
    }, [roster])

    return (
        <div>
            <h2>User team</h2>
            <div>Add, drop, create trade buttons go here
                <Button>Drop a player</Button>
                <Button>Create a trade</Button>
            </div>
            <div>Select other rosters goes to the side
                <ButtonDropdown>Other Rosters</ButtonDropdown>
            </div>
            <RosterCard roster={roster} scores={scores}/>
        </div>
    )
}