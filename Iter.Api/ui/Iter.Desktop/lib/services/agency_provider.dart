import '../models/agency.dart';
import 'base_provider.dart';

class AgencyProvider extends BaseProvider<Agency, Agency> {
  AgencyProvider() : super("Agency");

  @override
  Agency fromJson(data) {
    return Agency.fromJson(data);
  }

    @override
  Agency fromJsonSearch(data) {
    return Agency.fromJson(data);
  }
}