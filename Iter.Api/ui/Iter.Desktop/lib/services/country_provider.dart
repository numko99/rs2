
import 'package:ui/models/country.dart';

import 'base_provider.dart';

class CountryProvider extends BaseProvider<Country, Country> {
  CountryProvider() : super("Country");

  @override
  Country fromJson(data) {
    return Country.fromJson(data);
  }

    @override
  Country fromJsonSearch(data) {
    return Country.fromJson(data);
  }
}