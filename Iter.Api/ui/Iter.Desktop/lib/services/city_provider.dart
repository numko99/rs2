import 'package:ui/models/city.dart';

import 'base_provider.dart';

class CityProvider extends BaseProvider<City, City> {
  CityProvider() : super("City");

  @override
  City fromJson(data) {
    return City.fromJson(data);
  }

    @override
  City fromJsonSearch(data) {
    return City.fromJson(data);
  }
}