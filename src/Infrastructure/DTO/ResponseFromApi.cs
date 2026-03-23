// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Infrastructure.DTO;

public class ResponseFromApi
{
    public Search_Metadata search_metadata { get; set; }
    public Search_Parameters search_parameters { get; set; }
    public Best_Flights[] best_flights { get; set; }
    public Other_Flights[] other_flights { get; set; }
    public Price_Insights price_insights { get; set; }
    public Airport[] airports { get; set; }
}

public class Search_Metadata
{
    public string id { get; set; }
    public string status { get; set; }
    public string json_endpoint { get; set; }
    public string created_at { get; set; }
    public string processed_at { get; set; }
    public string google_flights_url { get; set; }
    public string raw_html_file { get; set; }
    public string prettify_html_file { get; set; }
    public float total_time_taken { get; set; }
}

public class Search_Parameters
{
    public string engine { get; set; }
    public string hl { get; set; }
    public string gl { get; set; }
    public string type { get; set; }
    public string departure_id { get; set; }
    public string arrival_id { get; set; }
    public string outbound_date { get; set; }
    public string currency { get; set; }
}

public class Price_Insights
{
    public int lowest_price { get; set; }
    public string price_level { get; set; }
    public int[] typical_price_range { get; set; }
    public int[][] price_history { get; set; }
}

public class Best_Flights
{
    public Flight[] flights { get; set; }
    public Layover[] layovers { get; set; }
    public int total_duration { get; set; }
    public Carbon_Emissions carbon_emissions { get; set; }
    public int price { get; set; }
    public string type { get; set; }
    public string airline_logo { get; set; }
    public string booking_token { get; set; }
}

public class Carbon_Emissions
{
    public int this_flight { get; set; }
    public int typical_for_this_route { get; set; }
    public int difference_percent { get; set; }
}

public class Flight
{
    public Departure_Airport departure_airport { get; set; }
    public Arrival_Airport arrival_airport { get; set; }
    public int duration { get; set; }
    public string airplane { get; set; }
    public string airline { get; set; }
    public string airline_logo { get; set; }
    public string travel_class { get; set; }
    public string flight_number { get; set; }
    public string legroom { get; set; }
    public string[] extensions { get; set; }
    public bool often_delayed_by_over_30_min { get; set; }
    public string[] ticket_also_sold_by { get; set; }
    public bool overnight { get; set; }
}

public class Departure_Airport
{
    public string name { get; set; }
    public string id { get; set; }
    public string time { get; set; }
}

public class Arrival_Airport
{
    public string name { get; set; }
    public string id { get; set; }
    public string time { get; set; }
}

public class Layover
{
    public int duration { get; set; }
    public string name { get; set; }
    public string id { get; set; }
    public bool overnight { get; set; }
}

public class Other_Flights
{
    public Flight1[] flights { get; set; }
    public Layover1[] layovers { get; set; }
    public int total_duration { get; set; }
    public Carbon_Emissions1 carbon_emissions { get; set; }
    public int price { get; set; }
    public string type { get; set; }
    public string airline_logo { get; set; }
    public string booking_token { get; set; }
}

public class Carbon_Emissions1
{
    public int this_flight { get; set; }
    public int typical_for_this_route { get; set; }
    public int difference_percent { get; set; }
}

public class Flight1
{
    public Departure_Airport1 departure_airport { get; set; }
    public Arrival_Airport1 arrival_airport { get; set; }
    public int duration { get; set; }
    public string airplane { get; set; }
    public string airline { get; set; }
    public string airline_logo { get; set; }
    public string travel_class { get; set; }
    public string flight_number { get; set; }
    public string[] ticket_also_sold_by { get; set; }
    public string legroom { get; set; }
    public string[] extensions { get; set; }
    public bool often_delayed_by_over_30_min { get; set; }
    public string plane_and_crew_by { get; set; }
    public bool overnight { get; set; }
}

public class Departure_Airport1
{
    public string name { get; set; }
    public string id { get; set; }
    public string time { get; set; }
}

public class Arrival_Airport1
{
    public string name { get; set; }
    public string id { get; set; }
    public string time { get; set; }
}

public class Layover1
{
    public int duration { get; set; }
    public string name { get; set; }
    public string id { get; set; }
    public bool overnight { get; set; }
}

public class Airport
{
    public Departure[] departure { get; set; }
    public Arrival[] arrival { get; set; }
}

public class Departure
{
    public Airport1 airport { get; set; }
    public string city { get; set; }
    public string country { get; set; }
    public string country_code { get; set; }
    public string image { get; set; }
    public string thumbnail { get; set; }
}

public class Airport1
{
    public string id { get; set; }
    public string name { get; set; }
}

public class Arrival
{
    public Airport2 airport { get; set; }
    public string city { get; set; }
    public string country { get; set; }
    public string country_code { get; set; }
    public string image { get; set; }
    public string thumbnail { get; set; }
}

public class Airport2
{
    public string id { get; set; }
    public string name { get; set; }
}
